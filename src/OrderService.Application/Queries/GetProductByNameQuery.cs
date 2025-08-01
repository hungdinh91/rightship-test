using MediatR;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.SharedKernel.Common;

namespace OrderService.Application.Queries;

public class GetProductByNameQuery : IRequest<Result<Product?>>
{
    public string Name { get; set; }

    public GetProductByNameQuery(string name)
    {
        Name = name;
    }
}

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, Result<Product?>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByNameQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Product?>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByNameAsync(request.Name);
        return Result.Ok(product);
    }
}
