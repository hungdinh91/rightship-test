using Microsoft.Extensions.Logging;
using OrderSystem.Domain.Shared.Common;
using OrderSystem.Domain.Shared.Dtos;

namespace OrderSystem.Application.Shared.Services;

public interface IOrderService
{
    Task<Result<OrderDto>> SubmitOrder(SubmittedOrderDto submittedOrderDto);
}

public class OrderService : IOrderService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<OrderService> _logger;
    private readonly string _httpClientName = "OrderServiceClient";

    public OrderService(IHttpClientService httpClientService, ILogger<OrderService> logger)
    {
        _httpClientService = httpClientService;
        _logger = logger;
    }

    public async Task<Result<OrderDto>> SubmitOrder(SubmittedOrderDto submittedOrderDto)
    {
        return await _httpClientService.PostAsync<OrderDto>(_httpClientName, "api/Orders/SubmitOrder", submittedOrderDto);
    }
}
