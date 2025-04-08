using MongoDB.Driver;
using SimpleInventoryManagementSystem.Models.Repositories.Interfaces;

namespace SimpleInventoryManagementSystem.models.Repositories.MongoDB;

public class ProductsMongoDbPersistence : IProductPersistence
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductsMongoDbPersistence(string connectionString, string databaseName, string collectionName)
    {
        var client = new MongoClient(connectionString); //"mongodb://localhost:27017"
        var database = client.GetDatabase(databaseName); //"SimpleInventoryManagementSystem"
        _productCollection = database.GetCollection<Product>(collectionName); //"Products"
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        try
        {
            return await _productCollection
                .Find(_ => true)
                .Project<Product>(Builders<Product>.Projection.Exclude("_id"))
                .ToListAsync();
        }
        catch (Exception error)
        {
            throw new Exception($"Error reading products: {error.Message}");
        }
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        try
        {
            await _productCollection.InsertOneAsync(product);
            return true;
        }
        catch (Exception error)
        {
            throw new Exception($"Error adding product: {error.Message}");
        }
    }

    public async Task<bool> EditProductAsync(string? oldProductName, Product updatedProduct)
    {
        try
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Name, updatedProduct.Name);
            var update = Builders<Product>.Update
                .Set(p => p.Name, updatedProduct.Name)
                .Set(p => p.Price, updatedProduct.Price)
                .Set(p => p.Quantity, updatedProduct.Quantity);
            var result = await _productCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception error)
        {
            throw new Exception($"Error editing product: {error.Message}");
        }
    }

    public async Task<bool> DeleteProductAsync(string? productName)
    {
        try
        {
            var result = await _productCollection.DeleteOneAsync(p => p.Name == productName);
            return result.DeletedCount > 0;
        }
        catch (Exception error)
        {
            throw new Exception($"Error deleting product: {error.Message}");
        }
    }
}