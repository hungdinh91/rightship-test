namespace OrderService.SharedKernel.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}
