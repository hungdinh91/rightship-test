using MediatR;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Dtos;

namespace OrderService.Application.Commands;

public class SubmitOrderCommand : IRequest<Result<OrderDto>>
{
    public SubmittedOrderDto SubmittedOrderDto { get; set; }

    public SubmitOrderCommand(SubmittedOrderDto submittedOrderDto)
    {
        SubmittedOrderDto = submittedOrderDto;
    }
}

public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public SubmitOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<OrderDto>> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        var productIds = request.SubmittedOrderDto.OrderItems.Select(x => x.ProductId).Distinct().ToList();
        var products = await _productRepository.GetByManyIdsAsync(productIds);
        var notExistProductIds = productIds.Where(x => !products.Any(y => y.Id == x)).ToList();

        if (notExistProductIds.Any())
        {
            var joinedIdString = string.Join(", ", notExistProductIds);
            return Result.Fail<OrderDto>(ErrorCode.ProductDoesNotExist, joinedIdString);
        }

        var orderItems = request.SubmittedOrderDto.OrderItems.Select(x =>
        {
            var product = products.First(p => p.Id == x.ProductId);
            return new OrderItem(product.Id, product.Price, x.Quantity);
        }).ToList();

        var order = new Order(request.SubmittedOrderDto.CustomerName, orderItems);

        await _orderRepository.AddAsync(order);

        var orderDto = new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Total = order.Total,
            OrderItems = order.OrderItems?.Select(x => new OrderItemDto { ProductId = x.ProductId, Price = x.Price, Quantity = x.Quantity })?.ToList()
                ?? new List<OrderItemDto>(),
        };

        return Result.Ok(orderDto);
    }
}
