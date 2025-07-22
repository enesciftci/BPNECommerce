using BPN.ECommerce.Domain.Aggregates.Orders.Entities;

namespace BPN.ECommerce.Application.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order> GetByOrderId(string orderId, CancellationToken cancellationToken);
}