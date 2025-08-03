using OrderSystem.Shared.Contracts;
using ProductService.Domain.Models;

namespace ProductService.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProductsAsync();
}
