namespace SimpleInventoryManagementSystem.models;

public interface IProductWriter
{
    bool AddProduct(Product product);
    bool EditProduct(string? oldProductName, Product updatedProduct);
    bool DeleteProduct(string? productName);
}
