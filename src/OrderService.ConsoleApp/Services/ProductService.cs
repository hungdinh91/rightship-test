using Microsoft.Extensions.Logging;
using OrderService.ConsoleApp.Helpers;
using OrderService.ConsoleApp.Settings;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Dtos;

namespace OrderService.ConsoleApp.Services;

public interface IProductService
{
    Task<Result<List<ProductDto>>> GetProductsAsync();
}

public class ProductService : IProductService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<ProductService> _logger;
    private readonly string _httpClientName;

    public ProductService(IHttpClientService httpClientService, ILogger<ProductService> logger, AppSettings appSettings)
    {
        _httpClientService = httpClientService;
        _logger = logger;
        _httpClientName = $"{appSettings.OrderApiHost}-Client";
    }

    public async Task<Result<List<ProductDto>>> GetProductsAsync()
    {
        return await _httpClientService.GetAsync<List<ProductDto>>(_httpClientName, "api/Products/GetProducts");
    }
}
