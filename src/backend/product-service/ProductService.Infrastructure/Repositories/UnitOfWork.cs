using OrderSystem.Shared.Contracts;
using ProductService.Infrastructure.DbContexts;

namespace ProductService.Infrastructure.Repositories;

public class UnitOfWork : OrderSystem.Infrastructure.Shared.Repositories.UnitOfWork<ProductDbContext>, IUnitOfWork, IDisposable
{
    public UnitOfWork(ProductDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}
