using OrderService.SharedKernel.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Dtos;

public class SubmittedOrderDto
{
    [Required]
    public required string CustomerName { get; set; }

    [NotEmpty]
    public required List<SubmittedOrderItemDto> OrderItems { get; set; }
}

public class SubmittedOrderItemDto
{
    [GuidNotEmpty]
    public Guid ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
