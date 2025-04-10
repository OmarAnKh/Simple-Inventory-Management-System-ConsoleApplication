using System.ComponentModel.DataAnnotations;

namespace SimpleInventoryManagementSystem.models;

class Inventory
{
    private static readonly Lock Lock = new Lock();
    private static Inventory? _instance;

    private readonly List<Product> _productsList;
    private readonly IProductWriter _productWriter;
    private readonly IProductPrint _productPrint;

    private Inventory()
    {
        var productReader = ProductPersistenceFactory.CreateReader();
        _productWriter = ProductPersistenceFactory.CreateWriter();
        _productPrint = new PrintProducts();
        _productsList = productReader.GetProducts();
    }

    public static Inventory GetInstance()
    {
        lock (Lock)
        {
            return _instance ??= new Inventory();
        }
    }

    public bool AddProduct(string? name, int quantity, int price)
    {
        var product = new Product(name, quantity, price);
        if (!IsProductValid(product))
        {
            return false;
        }

        if (!_productWriter.AddProduct(product))
        {
            return false;
        }

        _productsList.Add(product);
        return true;
    }

    public bool EditProduct(string? oldName, string? newName, int quantity, int price)
    {
        var updatedProduct = new Product(newName, quantity, price);

        if (!_productWriter.EditProduct(oldName, updatedProduct))
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

    public bool DeleteProduct(string? productName)
    {
        if (!_productWriter.DeleteProduct(productName))
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
        var product =
            _productsList.FirstOrDefault(p =>
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