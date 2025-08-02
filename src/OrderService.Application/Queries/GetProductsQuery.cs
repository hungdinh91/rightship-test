using MediatR;
using OrderService.Domain.Contracts.Repositories;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Dtos;

namespace OrderService.Application.Queries;

public class GetProductsQuery : IRequest<Result<List<ProductDto>>>
{
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsAsync();
        var productDtos = products.Select(x => new ProductDto { Id = x.Id, ProductName = x.ProductName, Price = x.Price }).ToList();
        return Result.Ok(productDtos);
    }
}
