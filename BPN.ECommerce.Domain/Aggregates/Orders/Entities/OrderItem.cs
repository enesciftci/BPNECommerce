using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Domain.Aggregates.Orders.Entities;

public class OrderItem : BaseEntity
{
    public string OrderId { get; private set; }
    public Order Order { get; private set; }
    // public Product.Entities.Product Product { get; set; }
    public string ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    private OrderItem(string orderId, string productId, int quantity, decimal price)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public static OrderItem Create(string orderId, string productId, int quantity, decimal price)
    {
        return new OrderItem(orderId, productId, quantity, price * quantity);
    }
}