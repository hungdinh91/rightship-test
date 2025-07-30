using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.Infrastructure.DbContexts;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext orderDbContext)
        {
            _dbContext = orderDbContext;
        }

        public void Create(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }
    }
}
