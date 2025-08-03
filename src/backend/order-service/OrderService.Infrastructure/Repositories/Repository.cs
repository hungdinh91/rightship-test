using OrderService.Infrastructure.DbContexts;
using OrderSystem.Domain.Shared.Models;
using OrderSystem.Shared.Contracts;

namespace OrderService.Infrastructure.Repositories;

public class Repository<T> : OrderSystem.Infrastructure.Shared.Repositories.Repository<OrderDbContext, T>, IRepository<T> where T : BaseEntity
{
    public Repository(OrderDbContext context) : base(context)
    {
    }
}
