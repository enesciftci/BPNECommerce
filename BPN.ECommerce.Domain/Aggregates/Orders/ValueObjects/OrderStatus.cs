using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;

public class OrderStatus : SimpleValueObject<string>
{
    private const string APPROVED = "Approved";
    private const string PENDING = "Pending";
    private const string CANCELLED = "Cancelled";
    
    private OrderStatus(string value) : base(value)
    {
    }
    
    public static OrderStatus Approved()
    {
        return new OrderStatus(APPROVED);
    }

    public static OrderStatus Pending()
    {
        return new OrderStatus(PENDING);
    }

    public static OrderStatus Cancelled()
    {
        return new OrderStatus(CANCELLED);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not OrderStatus other) return false;
        return Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}