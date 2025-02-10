namespace SimpleInventoryManagementSystem.models;

public class Product(string? name, int quantity, int price)
{
    public string? Name { get; set; } = name;
    public int Quantity { get; set; } = quantity;
    public int Price { get; set; } = price;
}