using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.Infrastructure.DbContexts;

namespace OrderService.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(OrderDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetByManyIdsAsync(IEnumerable<Guid> ids)
        {
            if (ids == null || !ids.Any()) return new List<Product>();
            return await _context.Products.Where(x => ids.Contains(x.Id)).ToListAsync();   
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
