using SimpleInventoryManagementSystem.models;

namespace SimpleInventoryManagementSystem.Models.Repositories.Interfaces;

public interface IProductWriter
{
    Task<bool> AddProductAsync(Product product);
    Task<bool> EditProductAsync(string? oldProductName, Product updatedProduct);
    Task<bool> DeleteProductAsync(string? productName);
}