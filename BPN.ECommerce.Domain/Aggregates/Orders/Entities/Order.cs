using BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;
using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Domain.Aggregates.Orders.Entities;

public class Order : BaseEntity
{
    public string OrderId { get; set; }
    public ICollection<OrderItem> Items { get; private set; }
    public decimal Amount { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    private Order(string orderId,ICollection<OrderItem> items, decimal amount, OrderStatus status, DateTime? completedAt, DateTime? cancelledAt)
    {
        OrderId = orderId;
        Items = items;
        Amount = amount;
        Status = status;
        CompletedAt = completedAt;
        CancelledAt = cancelledAt;
    }

    private Order()
    {
        
    }

    public static Order Create(string orderId, List<OrderItem> items, decimal amount, OrderStatus status)
    {
        return new Order(orderId, items, amount, status, null, null);
    }

    public void SetStatus(OrderStatus status)
    {
        Status = status;
    }
    
    public void SetCompletedAt(DateTime completedAt)
    {
        CompletedAt = completedAt;
    }
    
    public void SetCancelledAt(DateTime cancelledAt)
    {
        CancelledAt = cancelledAt;
    }
}