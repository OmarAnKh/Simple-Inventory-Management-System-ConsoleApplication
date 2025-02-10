namespace SimpleInventoryManagementSystem.Attributes;
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]

public class QuantityValidationAttribute: Attribute
{
    public string Message { get; }

    public QuantityValidationAttribute(string message)
    {
        Message = message;
    }
}