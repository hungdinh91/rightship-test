using OrderService.Infrastructure.DbContexts;
using OrderSystem.Shared.Contracts;

namespace OrderService.Infrastructure.Repositories;

public class UnitOfWork : OrderSystem.Infrastructure.Shared.Repositories.UnitOfWork<OrderDbContext>, IUnitOfWork, IDisposable
{
    public UnitOfWork(OrderDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}
