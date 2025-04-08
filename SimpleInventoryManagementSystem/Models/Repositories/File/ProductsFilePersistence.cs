using SimpleInventoryManagementSystem.models;
using SimpleInventoryManagementSystem.Models.Repositories.Interfaces;

namespace SimpleInventoryManagementSystem.Models.Repositories.File;

public class ProductsFilePersistence(string filePath) : IProductPersistence
{
    public async Task<List<Product>> GetProductsAsync()
    {
        var products = new List<Product>();

        try
        {
            var lines = await System.IO.File.ReadAllLinesAsync(filePath);
            products.AddRange(lines.Select(line => line.Split(","))
                .Select(data => new Product(data[0], int.Parse(data[1]), int.Parse(data[2]))));
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading products: {e.Message}");
        }

        return products;
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        try
        {
            var line = $"{product.Name},{product.Quantity},{product.Price}\n";
            await System.IO.File.AppendAllTextAsync(filePath, line);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error adding product: {e.Message}");
            return false;
        }
    }

    public async Task<bool> EditProductAsync(string? oldProductName, Product updatedProduct)
    {
        try
        {
            var lines = (await System.IO.File.ReadAllLinesAsync(filePath)).ToList();

            if (!SearchForProductAndUpdateIt(oldProductName, updatedProduct, lines))
            {
                return false;
            }

            await System.IO.File.WriteAllLinesAsync(filePath, lines);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error editing product: {e.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(string? productName)
    {
        try
        {
            var lines = (await System.IO.File.ReadAllLinesAsync(filePath)).ToList();
            var newLines = lines.Where(line => !line.StartsWith(productName + ",")).ToList();

            if (lines.Count == newLines.Count)
            {
                return false;
            }

            await System.IO.File.WriteAllLinesAsync(filePath, newLines);
            return true;
        }
        catch (Exception e)
        {
            throw new Exception($"Error deleting product: {e.Message}");
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

    private static bool SplitProductLine(string? oldProductName, string line)
    {
        var data = line.Split(",");
        return data[0] != oldProductName;
    }
}
