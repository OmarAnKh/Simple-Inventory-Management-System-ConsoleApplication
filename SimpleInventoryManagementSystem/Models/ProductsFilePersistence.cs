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
            Console.WriteLine($"Error reading products: {e.Message}");
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
            var updated = false;

            for (var i = 0; i < lines.Count; i++)
            {
                var data = lines[i].Split(",");

                if (data[0] != oldProductName)
                {
                    continue;
                }

                lines[i] = $"{updatedProduct.Name},{updatedProduct.Quantity},{updatedProduct.Price}";
                updated = true;
                break;
            }

            if (!updated) return false;
            File.WriteAllLines(FilePath, lines);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error editing product: {e.Message}");
            return false;
        }
    }

    public bool DeleteProduct(string? productName)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting product: {e.Message}");
            return false;
        }
    }
}