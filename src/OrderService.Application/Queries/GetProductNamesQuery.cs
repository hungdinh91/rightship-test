using MediatR;
using OrderService.Domain.Contracts.Repositories;
using OrderService.SharedKernel.Common;

namespace OrderService.Application.Queries;

public class GetProductNamesQuery : IRequest<Result<List<string>>>
{
}

public class GetProductNamesQueryHandler : IRequestHandler<GetProductNamesQuery, Result<List<string>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductNamesQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<string>>> Handle(GetProductNamesQuery request, CancellationToken cancellationToken)
    {
        var productNames = await _productRepository.GetProductNamesAsync();
        return Result.Ok(productNames);
    }
}
