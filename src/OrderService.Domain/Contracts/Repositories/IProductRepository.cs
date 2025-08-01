using OrderService.Domain.Models;

namespace OrderService.Domain.Contracts.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<string>> GetProductNamesAsync();
    Task<Product?> GetProductByNameAsync(string? productName);
    Task<List<Product>> GetByManyIdsAsync(IEnumerable<Guid> ids);
}
