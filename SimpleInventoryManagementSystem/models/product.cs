namespace SimpleInventoryManagementSystem.models;

public interface IProduct
{
    string? Name { get; set; }
    int Quantity { get; set; }
    int Price { get; set; }
    
}

public class Product(string? name, int quantity, int price) : IProduct
{
    public string? Name { get; set; } = name;
    public int Quantity { get; set; } = quantity;
    public int Price { get; set; } = price;
}