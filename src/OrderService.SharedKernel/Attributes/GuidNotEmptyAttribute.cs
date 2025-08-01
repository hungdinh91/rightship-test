using System.ComponentModel.DataAnnotations;

namespace OrderService.SharedKernel.Attributes;

public class GuidNotEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is Guid || value is Guid?)
        {
            return (Guid?)value != Guid.Empty;
        }

        return false;
    }

    public override string FormatErrorMessage(string name) =>
        $"{name} must be a valid non-empty GUID.";
}
