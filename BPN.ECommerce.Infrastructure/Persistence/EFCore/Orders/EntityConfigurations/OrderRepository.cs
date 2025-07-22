using BPN.ECommerce.Application.Orders;
using BPN.ECommerce.Domain.Aggregates.Orders.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPN.ECommerce.Infrastructure.Persistence.EFCore.Orders.EntityConfigurations;

public class OrderRepository : IOrderRepository
{
    private readonly BpnDbContext _bpnDbContext;

    public OrderRepository(BpnDbContext bpnDbContext)
    {
        _bpnDbContext = bpnDbContext;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _bpnDbContext.Orders.AddAsync(order, cancellationToken);
    }
    
    public async Task<Order> GetByOrderId(string orderId, CancellationToken cancellationToken)
    {
        return await _bpnDbContext.Orders.FirstOrDefaultAsync(p=>p.OrderId == orderId, cancellationToken);
    }
}