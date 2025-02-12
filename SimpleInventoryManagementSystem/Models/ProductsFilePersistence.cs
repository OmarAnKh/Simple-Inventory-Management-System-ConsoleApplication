namespace SimpleInventoryManagementSystem.models;

public class ProductsFilePersistence : IProductReader, IProductWriter
{
    private const string FilePath = "../../../Data/products.txt";

    public List<Product> GetProducts()
    {
        var products = new List<Product>();

        try
        {
            products.AddRange(File.ReadAllLines(FilePath).Select(line => line.Split(","))
                .Select(data => new Product(data[0], int.Parse(data[1]), int.Parse(data[2]))));
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading products: {e.Message}");
        }

        return products;
    }

    public bool AddProduct(Product product)
    {
        try
        {
            File.AppendAllText(FilePath, $"{product.Name},{product.Quantity},{product.Price}\n");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error adding product: {e.Message}");
            return false;
        }
    }

    public bool EditProduct(string? oldProductName, Product updatedProduct)
    {
        try
        {
            var lines = File.ReadAllLines(FilePath).ToList();

            if (!SearchForProductAndUpdateIt(oldProductName, updatedProduct, lines))
            {
                return false;
            }
            File.WriteAllLines(FilePath, lines);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error editing product: {e.Message}");
            return false;
        }
    }

    private static bool SearchForProductAndUpdateIt(string? oldProductName, Product updatedProduct, List<string> lines)
    {
        for (var index = 0; index < lines.Count; index++)
        {
            if (SplitProductLine(oldProductName, lines[index])) continue;

            lines[index] = $"{updatedProduct.Name},{updatedProduct.Quantity},{updatedProduct.Price}";
            return true;
        }

        return false;
    }

    private static bool SplitProductLine(string? oldProductName, String lines)
    {
        var data = lines.Split(",");

        return data[0] != oldProductName;
    }

    public bool DeleteProduct(string? productName)
    {
        try
        {
            return RemoveProductFromFileList(productName);
        }
        catch (Exception e)
        {
            throw new Exception($"Error deleting product: {e.Message}");
        }
    }

    private static bool RemoveProductFromFileList(string? productName)
    {
        var lines = File.ReadAllLines(FilePath).ToList();
        var newLines = lines.Where(line => !line.StartsWith(productName + ",")).ToList();

        if (lines.Count == newLines.Count)
        {
            return false;
        }

        File.WriteAllLines(FilePath, newLines);
        return true;
    }
}