namespace SimpleInventoryManagementSystem.models;

public class ProductsFilePersistence : IProductReader, IProductWriter
{
    private readonly string _filePath = "../../../Data/products.txt";

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

            if (lines.Count == newLines.Count)
            {
                return false;
            }

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
