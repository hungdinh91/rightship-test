using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.Infrastructure.DbContexts;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
