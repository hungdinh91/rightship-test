using OrderSystem.Domain.Shared.Models;

namespace ProductService.Domain.Models;

public class Product : BaseEntity
{
    public required string ProductName { get; set; }
    public double Price { get; set; }
}
