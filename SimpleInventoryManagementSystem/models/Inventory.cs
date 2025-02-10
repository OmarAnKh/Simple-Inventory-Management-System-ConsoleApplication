namespace SimpleInventoryManagementSystem.models;
public class Inventory
{
    private static readonly Lock Lock = new Lock();
    private static Inventory? _instance;

    private readonly List<Product> _productsList;
    private readonly IProductReader _productReader;
    private readonly IProductWriter _productWriter;
    private readonly IProductPrint _productPrint;

    private Inventory()
    {
        _productReader =  ProductPersistenceFactory.CreateReader();
        _productWriter =  ProductPersistenceFactory.CreateWriter();
        _productPrint = new PrintProducts();
        _productsList = _productReader.GetProducts();
    }

    public static Inventory GetInstance()
    {
        lock (Lock)
        {
            return _instance ??= new Inventory();
        }
    }

    public bool AddProduct(string name, int quantity, int price)
    {
        var product = new Product(name, quantity, price);

        if (!_productWriter.AddProduct(product))
        {
            return false;
        }

        _productsList.Add(product);
        return true;
    }

    public bool EditProduct(string oldName, string newName, int quantity, int price)
    {
        var updatedProduct = new Product(newName, quantity, price);

        if (!_productWriter.EditProduct(oldName, updatedProduct))
        {
            return false;
        }

        var product = _productsList.FirstOrDefault(p => p.Name == oldName);
        if (product != null)
        {
            product.Name = newName;
            product.Quantity = quantity;
            product.Price = price;
        }

        return true;
    }

    public bool DeleteProduct(string productName)
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

    public void Search(string searchTerm)
    {
        var product = _productsList.FirstOrDefault(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        if (product != null)
        {
            Console.WriteLine($"Product found: {product.Name}, Quantity: {product.Quantity}, Price: {product.Price}");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }
}