namespace SimpleInventoryManagementSystem.models;

public interface IProductsPersistence
{
    List<Product?> GetProducts();
    bool EditProduct(Product product);
    bool AddProduct(Product? product); 
    
}

public interface IProductsPrint
{
    void Print(List<Product?> products);
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
    public bool EditProduct(Product product)
    {
        try
        {
            var lines = File.ReadAllLines("../../../Files/products.txt").ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                string[] data = lines[i].Split(',');

                if (data[0] == product.Name)
                {
                    data[1] = product.Price.ToString();
                    data[2] = product.Quantity.ToString();

                    lines[i] = string.Join(",", data);
                    break; 
                }
            }

            File.WriteAllLines("../../../Files/products.txt", lines);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false; 
        }
    }


    public bool AddProduct(Product? product)
    {
        try
        {
            string filePath = "../../../Files/products.txt"; 

            using (StreamWriter writer = new StreamWriter(filePath, append: true))
            {
                writer.WriteLine($"{product.Name},{product.Quantity},{product.Price}"); 
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
    public List<Product?> GetProducts()
    {
        List<Product?> products = new List<Product?>();
        try
        {
            StreamReader sr = new StreamReader("../../../Files/products.txt");
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

public class PrintProducts : IProductsPrint
{
    public void Print(List<Product?> products)
    {
        foreach (var prod in products)
        {
            Console.WriteLine($"Product name:{prod.Name}, Quantity:{prod.Quantity}, Price:{prod.Price}" );
        }
    }
}


public class Products
{
    private static readonly Lock Lock = new Lock();
    private static  Products? _products;
    private readonly List<Product?> _productsList;
    private readonly IProductsPersistence _productsFilePersistence;
    private readonly IProductsPrint _productsPrint;
    private Products()
    {
        _productsFilePersistence = new ProductsFilePersistence();
        _productsList = _productsFilePersistence.GetProducts();    
        _productsPrint=new PrintProducts();
    }

    public static Products? GetInstance()
    {
        lock (Lock)
        {
            _products ??= new Products();
        }

        return _products;
    }

    public bool AddProduct(string productName, int productQuantity, int productPrice)
    {
        var product = new Product(productName, productQuantity, productPrice);
        if (!_productsFilePersistence.AddProduct(product)) return false;
        _productsList.Add(product);
        return true;

    }

    public void Print(List<Product?>? products = null)
    {
        if (products != null)
        {
            _productsPrint.Print(products);
        }
        _productsPrint.Print(_productsList);
    }

    public bool EditProduct(string productName, int productQuantity, int productPrice)
    {
        if (_productsFilePersistence.EditProduct(new Product(productName, productQuantity, productPrice)))
        {
            var prod= _productsList.FirstOrDefault(p => p?.Name == productName);
            if (prod == null) return false;
            prod.Quantity = productQuantity;
            prod.Price = productPrice;
            
        }

        return true;
    }

}