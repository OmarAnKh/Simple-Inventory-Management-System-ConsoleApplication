using System.ComponentModel.DataAnnotations;

namespace SimpleInventoryManagementSystem.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
class QuantityValidationAttribute(string message) : ValidationAttribute(message)
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int price && price > 0)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }
}