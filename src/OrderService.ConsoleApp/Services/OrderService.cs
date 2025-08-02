using Microsoft.Extensions.Logging;
using OrderService.ConsoleApp.Helpers;
using OrderService.ConsoleApp.Settings;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Dtos;

namespace OrderService.ConsoleApp.Services;

public interface IOrderService
{
    Task<Result<OrderDto>> SubmitOrder(SubmittedOrderDto submittedOrderDto);
}

public class OrderService : IOrderService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<OrderService> _logger;
    private readonly string _httpClientName;

    public OrderService(IHttpClientService httpClientService, ILogger<OrderService> logger, AppSettings appSettings)
    {
        _httpClientService = httpClientService;
        _logger = logger;
        _httpClientName = $"{appSettings.OrderApiHost}-Client";
    }

    public async Task<Result<OrderDto>> SubmitOrder(SubmittedOrderDto submittedOrderDto)
    {
        return await _httpClientService.PostAsync<OrderDto>(_httpClientName, "api/Orders/SubmitOrder", submittedOrderDto);
    }
}
