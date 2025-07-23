using BPN.ECommerce.Application.Orders.Notifications.VoidPayment;
using BPN.ECommerce.Application.Services.Balance.Requests;

namespace BPN.ECommerce.Application.Orders.Mapper;

public interface IOrderMapper
{
    InitPaymentRequest MapToInitPaymentRequest(string orderId, decimal amount);
    AuthPaymentRequest MapToAuthPaymentRequest(string orderId);
    VoidPaymentRequest MapToVoidPaymentRequest(string orderId);
    VoidPaymentNotification MapToVoidPaymentNotification(string orderId);
}
public class OrderMapper : IOrderMapper
{
    public InitPaymentRequest MapToInitPaymentRequest(string orderId, decimal amount)
    {
        return new InitPaymentRequest()
        {
            OrderId = orderId,
            Amount = amount,
        };
    }
    
    public AuthPaymentRequest MapToAuthPaymentRequest(string orderId)
    {
        return new AuthPaymentRequest()
        {
            OrderId = orderId
        };
    }
    
    public VoidPaymentRequest MapToVoidPaymentRequest(string orderId)
    {
        return new VoidPaymentRequest()
        {
            OrderId = orderId
        };
    }
    
    public VoidPaymentNotification MapToVoidPaymentNotification(string orderId)
    {
        return new VoidPaymentNotification()
        {
            OrderId = orderId
        };
    }
}