using BPN.ECommerce.Application.Orders.Exceptions;
using BPN.ECommerce.Application.Orders.Inputs;
using BPN.ECommerce.Application.Orders.Mapper;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BPN.ECommerce.Application.Orders.Commands.CompleteOrder;

public class CompleteOrderCommand : IRequest
{
    public CompleteOrderInput Input { get; set; }

    public CompleteOrderCommand(CompleteOrderInput input)
    {
        Input = input;
    }

    public static CompleteOrderCommand Create(CompleteOrderInput input)
    {
        return new CompleteOrderCommand(input);
    }
}

public class CompleteOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IBalanceServiceClient balanceService,
    IOrderMapper orderMapper,
    IOrderRepository orderRepository,
    IMediator mediator,
    ILogger<CompleteOrderCommandHandler> logger)
    : IRequestHandler<CompleteOrderCommand>
{
    public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
       var preOrder = await orderRepository.GetByOrderId(request.Input.OrderId, cancellationToken);

       if (preOrder == null)
       {
           throw new PreOrderNotFoundException("Pre order not found");
       }
       
       var authRequest = orderMapper.MapToAuthPaymentRequest(request.Input.OrderId);
       var authPaymentResponse = await balanceService.AuthPayment(authRequest, cancellationToken);
       
       if (!authPaymentResponse.Success)
       {
           logger.LogWarning("Payment failed for OrderId: {OrderId}. Reason: {Reason}",
               request.Input.OrderId, authPaymentResponse.Message);
           
           var voidPaymentNotification = orderMapper.MapToVoidPaymentNotification(request.Input.OrderId);
           
           await mediator.Publish(voidPaymentNotification, cancellationToken);
           
           throw new AuthPaymentException(authPaymentResponse.Message);
       }
       
       if (authPaymentResponse.Data?.Order is not null)
       {
           var completedOrder = authPaymentResponse.Data.Order;
           preOrder.SetStatus(OrderStatus.Approved());
           preOrder.SetCompletedAt(completedOrder.CompletedAt.Value);
          
           await unitOfWork.SaveChangesAsync(cancellationToken);
       }
    }
}