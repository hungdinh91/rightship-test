using OrderSystem.Domain.Shared.Models;

namespace OrderService.Domain.Models;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public Order? Order { get; set; }

    private OrderItem() { }
    public OrderItem(Guid productId, double price, int quantity)
    {
        ProductId = productId;
        Price = price;
        Quantity = quantity;
        CreatedAt = DateTimeOffset.Now;
    }
}
