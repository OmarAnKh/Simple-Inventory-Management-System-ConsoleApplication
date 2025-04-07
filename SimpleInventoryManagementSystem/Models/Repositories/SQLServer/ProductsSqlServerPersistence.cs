using System.Data.SqlClient;
using SimpleInventoryManagementSystem.Models.Repositories.Interfaces;

namespace SimpleInventoryManagementSystem.models.Repositories.SQLServer;

public class ProductsSqlServerPersistence : IProductPersistence
{
    private string connectionString ="Server=localhost;Database=InventoryDb;Trusted_Connection=True";
    
    public async Task<List<Product>> GetProductsAsync()
    {
        var products = new List<Product>();
        
        const string query = "SELECT Name, Quantity, Price FROM Products";

        await using var connection = new SqlConnection(connectionString);
        await using var command = new SqlCommand(query, connection);
        
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(new Product(
                reader.GetString(0),
                reader.GetInt32(1),
                reader.GetInt32(2)
                ));
        }
        return products;
    }
    public async Task<bool> AddProductAsync(Product product)
    {
        string query ="INSERT INTO Products (Name, Quantity, Price) VALUES (@Name, @Quantity, @Price)";
        
        await using var connection = new SqlConnection(connectionString);
        await using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Quantity", product.Quantity);
        command.Parameters.AddWithValue("@Price", product.Price);
        
        await connection.OpenAsync();
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }
    public async Task<bool> EditProductAsync(string? oldProductName, Product updatedProduct)
    {
        const string query = "UPDATE Products SET Name = @NewName, Quantity = @Quantity, Price = @Price WHERE Name = @OldName";

        await using var connection = new SqlConnection(connectionString);
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@NewName", updatedProduct.Name);
        command.Parameters.AddWithValue("@Quantity", updatedProduct.Quantity);
        command.Parameters.AddWithValue("@Price", updatedProduct.Price);
        command.Parameters.AddWithValue("@OldName", oldProductName);

        await connection.OpenAsync();
        var result = await command.ExecuteNonQueryAsync();

        return result > 0;
    }
  public async Task<bool> DeleteProductAsync(string? productName)
    {
        const string query = "DELETE FROM Products WHERE Name = @Name";

        await using var connection = new SqlConnection(connectionString);
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", productName);

        await connection.OpenAsync();
        var result = await command.ExecuteNonQueryAsync();

        return result > 0;
    }
    
}
