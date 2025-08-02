using MediatR;
using OrderService.Domain.Contracts;
using OrderService.Domain.Contracts.Repositories;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Constants;
using OrderService.SharedKernel.Dtos;

namespace OrderService.Application.Queries;

public class GetProductsQuery : IRequest<Result<List<ProductDto>>>
{
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IRedisCache _redisCache;

    public GetProductsQueryHandler(IProductRepository productRepository, IRedisCache redisCache)
    {
        _productRepository = productRepository;
        _redisCache = redisCache;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        // Try to get from redis cache first
        var cachedProducts = await _redisCache.GetAsync<List<ProductDto>>(CacheKeys.Products);
        if (cachedProducts != null)
        {
            return Result.Ok(cachedProducts);
        }

        var products = await _productRepository.GetProductsAsync();
        var productDtos = products.Select(x => new ProductDto { Id = x.Id, ProductName = x.ProductName, Price = x.Price }).ToList();

        await _redisCache.SetAsync(CacheKeys.Products, productDtos);

        return Result.Ok(productDtos);
    }
}
