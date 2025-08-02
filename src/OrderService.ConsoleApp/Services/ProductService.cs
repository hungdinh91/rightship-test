using Microsoft.Extensions.Logging;
using OrderService.ConsoleApp.Helpers;
using OrderService.ConsoleApp.Settings;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Dtos;
using System.Text.Encodings.Web;
using System.Web;

namespace OrderService.ConsoleApp.Services;

public interface IProductService
{
    Task<Result<ProductDto>> GetProductByNameAsync(string? productName);
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

    public async Task<Result<ProductDto>> GetProductByNameAsync(string? productName)
    {
        return await _httpClientService.GetAsync<ProductDto>(_httpClientName, $"api/Products/GetProductByName/{HttpUtility.UrlEncode(productName)}");
    }

    public async Task<Result<List<ProductDto>>> GetProductsAsync()
    {
        return await _httpClientService.GetAsync<List<ProductDto>>(_httpClientName, "api/Products/GetProducts");
    }
}
