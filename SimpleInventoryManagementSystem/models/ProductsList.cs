namespace SimpleInventoryManagementSystem.models;

public interface IProductsPersistence
{
    List<Product> GetProducts();
    
}
public interface IProductsListManager
{
    bool AddProduct(Product product);
}


public class ProductsListManager : IProductsListManager
{
    public bool AddProduct(Product product)
    {
        throw new NotImplementedException();
    }
}

public class ProductsFilePersistence : IProductsPersistence
{
    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        try
        {
            StreamReader sr = new StreamReader("products.txt");
            string? line= sr.ReadLine();
            while (line!=null)
            {
                string[] product=line.Split(",");
                products.Add(new Product(product[0],Int32.Parse(product[1]),Int32.Parse(product[2])));
                line = sr.ReadLine();
            }
            sr.Close();
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't Read from the file: {e.Message}");
            System.Environment.Exit(1);
        }

        return products;
    }
}
public class Products
{
    private static readonly object _lock = new object();
    private static  Products _products;
    private readonly List<Product> _productsList;
    private Products()
    {
        _productsList = new List<Product>();    
    }

    public static Products GetInstance()
    {
        lock (_lock)
        {
            _products ??= new Products();
        }

        return _products;
    }
}