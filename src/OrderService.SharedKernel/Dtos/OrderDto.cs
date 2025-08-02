namespace OrderService.SharedKernel.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public double Total { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
