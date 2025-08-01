using MediatR;
using OrderService.Application.Dtos;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.SharedKernel.Common;

namespace OrderService.Application.Commands;

public class SubmitOrderCommand : IRequest<Result<Order>>
{
    public SubmittedOrderDto SubmittedOrderDto { get; set; }

    public SubmitOrderCommand(SubmittedOrderDto submittedOrderDto)
    {
        SubmittedOrderDto = submittedOrderDto;
    }
}

public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, Result<Order>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public SubmitOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<Order>> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        var productIds = request.SubmittedOrderDto.OrderItems.Select(x => x.ProductId).Distinct().ToList();
        var products = await _productRepository.GetByManyIdsAsync(productIds);
        var notExistProductIds = productIds.Where(x => !products.Any(y => y.Id == x)).ToList();

        if (notExistProductIds.Any())
        {
            var joinedIdString = string.Join(", ", notExistProductIds);
            return Result.Fail<Order>(ErrorCode.ProductDoesNotExist, joinedIdString);
        }

        var orderItems = request.SubmittedOrderDto.OrderItems.Select(x =>
        {
            var product = products.First(p => p.Id == x.ProductId);
            return new OrderItem(product.Id, product.Price, x.Quantity);
        }).ToList();

        var order = new Order(request.SubmittedOrderDto.CustomerName, orderItems);

        await _orderRepository.AddAsync(order);

        return Result.Ok(order);
    }
}
