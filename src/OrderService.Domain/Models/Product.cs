namespace OrderService.Domain.Models;

public class Product : BaseEntity
{
    public required string ProductName { get; set; }
    public double Price { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
