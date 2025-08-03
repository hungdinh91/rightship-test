using System.ComponentModel;

namespace OrderSystem.Domain.Shared.Common;

public enum ErrorCode
{
    Undefined = 0,
    ItemNotFound = 1,

    [Description("API Connection error")]
    ApiConnectionError = 10000,

    [Description("Product {0} does not exist")]
    ProductDoesNotExist = 10001,
}
