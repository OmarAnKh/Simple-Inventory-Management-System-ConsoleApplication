using System.ComponentModel.DataAnnotations;
using SimpleInventoryManagementSystem.Models.Repositories.Interfaces;

namespace SimpleInventoryManagementSystem.models;

class Inventory
{
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
    private static Inventory? _instance;

    private readonly List<Product> _productsList;
    private readonly IProductPersistence _productPersistence;
    private readonly IProductPrint _productPrint;

    private Inventory(IProductPersistence persistence, List<Product> products)
    {
        _productPersistence = persistence;
        _productsList = products;
        _productPrint = new PrintProducts();
    }

    public static async Task<Inventory> CreateAsync()
    {
        if (_instance != null) return _instance;

        await Semaphore.WaitAsync();
        try
        {
            if (_instance == null)
            {
                var persistence = ProductPersistenceFactory.CreatePersistence();
                var products = await persistence.GetProductsAsync();
                _instance = new Inventory(persistence, products);
            }
        }
        finally
        {
            Semaphore.Release();
        }

        return _instance;
    }

    public async Task<bool> AddProductAsync(string? name, int quantity, int price)
    {
        var product = new Product(name, quantity, price);
        if (!IsProductValid(product))
        {
            return false;
        }

        if (!await _productPersistence.AddProductAsync(product))
        {
            return false;
        }

        _productsList.Add(product);
        return true;
    }

    public async Task<bool> EditProductAsync(string? oldName, string? newName, int quantity, int price)
    {
        var updatedProduct = new Product(newName, quantity, price);

        if (!await _productPersistence.EditProductAsync(oldName, updatedProduct))
        {
            return false;
        }

        var product = _productsList.FirstOrDefault(p => p.Name == oldName);
        if (product == null) return true;

        product.Name = newName;
        product.Quantity = quantity;
        product.Price = price;

        return true;
    }

    public async Task<bool> DeleteProductAsync(string? productName)
    {
        if (!await _productPersistence.DeleteProductAsync(productName))
        {
            return false;
        }

        _productsList.RemoveAll(p => p.Name == productName);
        return true;
    }

    public void Print()
    {
        _productPrint.Print(_productsList);
    }

    public void Search(string? searchTerm)
    {
        var product = _productsList.FirstOrDefault(p =>
            p.Name != null && p.Name.Contains(searchTerm ?? string.Empty, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine(product != null
            ? $"Product found: {product.Name}, Quantity: {product.Quantity}, Price: {product.Price}"
            : "Product not found.");
    }

    private static bool IsProductValid(Product product)
    {
        var context = new ValidationContext(product, null, null);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(product, context, results, true);

        if (!isValid)
        {
            foreach (ValidationResult result in results)
            {
                Console.WriteLine(result.ErrorMessage);
            }
        }

        return isValid;
    }
}