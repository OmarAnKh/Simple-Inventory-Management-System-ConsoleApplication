namespace SimpleInventoryManagementSystem.models;

public interface IProductReader
{
    List<Product> GetProducts();
}

public interface IProductWriter
{
    bool AddProduct(Product product);
    bool EditProduct(string oldProductName, Product updatedProduct);
    bool DeleteProduct(string productName);
}

public interface IProductPrint
{
    void Print(List<Product> products);
}


public class ProductsFilePersistence : IProductReader, IProductWriter
{
    private readonly string _filePath = "../../../Files/products.txt";

    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        try
        {
            foreach (var line in File.ReadAllLines(_filePath))
            {
                var data = line.Split(",");
                products.Add(new Product(data[0], int.Parse(data[1]), int.Parse(data[2])));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading products: {e.Message}");
        }

        return products;
    }

    public bool AddProduct(Product product)
    {
        try
        {
            File.AppendAllText(_filePath, $"{product.Name},{product.Quantity},{product.Price}\n");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error adding product: {e.Message}");
            return false;
        }
    }

    public bool EditProduct(string oldProductName, Product updatedProduct)
    {
        try
        {
            var lines = File.ReadAllLines(_filePath).ToList();
            bool updated = false;

            for (int i = 0; i < lines.Count; i++)
            {
                var data = lines[i].Split(",");

                if (data[0] == oldProductName)
                {
                    lines[i] = $"{updatedProduct.Name},{updatedProduct.Quantity},{updatedProduct.Price}";
                    updated = true;
                    break;
                }
            }

            if (updated)
            {
                File.WriteAllLines(_filePath, lines);
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error editing product: {e.Message}");
            return false;
        }
    }

    public bool DeleteProduct(string productName)
    {
        try
        {
            var lines = File.ReadAllLines(_filePath).ToList();
            var newLines = lines.Where(line => !line.StartsWith(productName + ",")).ToList();

            if (lines.Count == newLines.Count) return false;

            File.WriteAllLines(_filePath, newLines);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting product: {e.Message}");
            return false;
        }
    }
}


public static class ProductPersistenceFactory
{
    public static IProductReader CreateReader()
    {
        return new ProductsFilePersistence();
    }

    public static IProductWriter CreateWriter()
    {
        return new ProductsFilePersistence();
    }
}

public class PrintProducts : IProductPrint
{
    public void Print(List<Product> products)
    {
        foreach (var prod in products)
        {
            Console.WriteLine($"Product name: {prod.Name}, Quantity: {prod.Quantity}, Price: {prod.Price}");
        }
    }
}




public class Inventory
{
    private static readonly object _lock = new object();
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
        lock (_lock)
        {
            return _instance ??= new Inventory();
        }
    }

    public bool AddProduct(string name, int quantity, int price)
    {
        var product = new Product(name, quantity, price);

        if (!_productWriter.AddProduct(product)) return false;

        _productsList.Add(product);
        return true;
    }

    public bool EditProduct(string oldName, string newName, int quantity, int price)
    {
        var updatedProduct = new Product(newName, quantity, price);

        if (!_productWriter.EditProduct(oldName, updatedProduct)) return false;

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
        if (!_productWriter.DeleteProduct(productName)) return false;

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