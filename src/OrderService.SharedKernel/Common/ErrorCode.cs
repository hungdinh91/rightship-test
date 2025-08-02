using System.ComponentModel;

namespace OrderService.SharedKernel.Common;

public enum ErrorCode
{
    ItemNotFound = 1,

    [Description("API Connection error")]
    ApiConnectionError = 10000,

    [Description("Product {0} does not exist")]
    ProductDoesNotExist = 10001,
}
