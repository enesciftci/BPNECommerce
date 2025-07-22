using BPN.ECommerce.Application.Orders.Exceptions;
using BPN.ECommerce.Application.Orders.Mapper;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BPN.ECommerce.Application.Orders.Notifications.VoidPayment;

public class VoidPaymentNotification : INotification
{
    public string OrderId { get; set; }
}

public class VoidPaymentNotificationHandler(
    IBalanceServiceClient balanceServiceClient,
    IOrderMapper orderMapper,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    ILogger<VoidPaymentNotificationHandler> logger)
    : INotificationHandler<VoidPaymentNotification>
{
    public async Task Handle(VoidPaymentNotification notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByOrderId(notification.OrderId, cancellationToken);

        if (order == null)
        {
            throw new PreOrderNotFoundException("Pre order not found");
        }

        var voidPaymentRequest = orderMapper.MapToVoidPaymentRequest(notification.OrderId);
        var voidPaymentResponse = await balanceServiceClient.VoidPayment(voidPaymentRequest, cancellationToken);
        
        if (voidPaymentResponse.Success && voidPaymentResponse.Data?.Order is not null)
        {
            var cancelledOrder = voidPaymentResponse.Data.Order;
            order.SetStatus(OrderStatus.Cancelled());
            order.SetCancelledAt(cancelledOrder.CancelledAt.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation("Order {OrderId} successfully cancelled after payment failure", notification.OrderId);
        }
    }
}