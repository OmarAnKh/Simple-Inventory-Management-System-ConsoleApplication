namespace SimpleInventoryManagementSystem.models;
using Attributes;
public class Product(string? name, int quantity, int price)
{
    public string? Name { get; set; } = name;
    [QuantityValidation("Quantity must be zero or more.")]
    public int Quantity { get; set; } = quantity;
    [PriceValidation("Price must be greater than zero.")]
    public decimal Price { get; set; } = price;
}