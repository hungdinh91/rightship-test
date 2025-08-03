using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Shared.Attributes;

public class NotEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var list = value as ICollection;
        return list != null && list.Count > 0;
    }

    public override string FormatErrorMessage(string name)
        => $"{name} must contain at least one item.";
}
