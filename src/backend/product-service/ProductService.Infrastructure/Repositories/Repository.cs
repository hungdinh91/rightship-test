using OrderSystem.Domain.Shared.Models;
using OrderSystem.Shared.Contracts;
using ProductService.Infrastructure.DbContexts;

namespace ProductService.Infrastructure.Repositories;

public class Repository<T> : OrderSystem.Infrastructure.Shared.Repositories.Repository<ProductDbContext, T>, IRepository<T> where T : BaseEntity
{
    public Repository(ProductDbContext context) : base(context)
    {
    }
}
