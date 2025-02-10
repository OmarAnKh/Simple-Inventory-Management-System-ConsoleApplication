namespace SimpleInventoryManagementSystem.models;
using SimpleInventoryManagementSystem.Attributes;
public class Product(string? name, int quantity, int price)
{
    public string? Name { get; set; } = name;
    public int Quantity { get; set; } = quantity;
    [PriceValidationAttribute("Price must be greater than zero.")]
    public decimal Price { get; set; } = price;
}