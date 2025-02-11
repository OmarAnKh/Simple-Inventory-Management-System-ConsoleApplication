namespace SimpleInventoryManagementSystem.Attributes;
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class PriceValidationAttribute(string message) : Attribute
{
    public string Message { get; } = message;
}