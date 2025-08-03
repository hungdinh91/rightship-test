using Microsoft.Extensions.Logging;
using OrderSystem.Domain.Shared.Common;
using OrderSystem.Domain.Shared.Dtos;

namespace OrderSystem.Application.Shared.Services;

public interface IProductService
{
    Task<Result<List<ProductDto>>> GetProductsAsync();
}

public class ProductService : IProductService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<ProductService> _logger;
    private readonly string _httpClientName = "ProductServiceClient";

    public ProductService(IHttpClientService httpClientService, ILogger<ProductService> logger)
    {
        _httpClientService = httpClientService;
        _logger = logger;
    }

    public async Task<Result<List<ProductDto>>> GetProductsAsync()
    {
        return await _httpClientService.GetAsync<List<ProductDto>>(_httpClientName, "api/Products/GetProducts");
    }
}
