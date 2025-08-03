using OrderService.Domain.Models;
using OrderSystem.Shared.Contracts;

namespace OrderService.Domain.Contracts.Repositories;

public interface IOrderRepository : IRepository<Order>
{
}
