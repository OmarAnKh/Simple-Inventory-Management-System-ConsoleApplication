using SimpleInventoryManagementSystem.models;

namespace SimpleInventoryManagementSystem.Models.Repositories.Interfaces;

public interface IProductReader
{
    Task<List<Product>> GetProductsAsync();
}