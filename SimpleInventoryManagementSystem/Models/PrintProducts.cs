namespace SimpleInventoryManagementSystem.models;

class PrintProducts : IProductPrint
{
    public void Print(List<Product> products)
    {
        foreach (var prod in products)
        {
            Console.WriteLine($"Product name: {prod.Name}, Quantity: {prod.Quantity}, Price: {prod.Price}");
        }
    }
}
