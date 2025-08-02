using OrderService.Domain.Models;

namespace OrderService.Domain.Contracts.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProductsAsync();
    Task<List<Product>> GetByManyIdsAsync(IEnumerable<Guid> ids);
}
