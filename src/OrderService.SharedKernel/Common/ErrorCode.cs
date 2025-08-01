using System.ComponentModel;

namespace OrderService.SharedKernel.Common;

public enum ErrorCode
{
    [Description("Product {0} does not exist")]
    ProductDoesNotExist = 10001,
}
