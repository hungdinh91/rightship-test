using OrderService.Domain.Models;

namespace OrderService.Domain.Contracts.Repositories;

public interface IOrderRepository
{
    void Create(Order order);
}
