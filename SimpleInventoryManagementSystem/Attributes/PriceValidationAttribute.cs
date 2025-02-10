namespace SimpleInventoryManagementSystem.Attributes;
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class PriceValidationAttribute : Attribute
{
    public string Message { get; }

    public PriceValidationAttribute(string message)
    {
        Message = message;
    }
}