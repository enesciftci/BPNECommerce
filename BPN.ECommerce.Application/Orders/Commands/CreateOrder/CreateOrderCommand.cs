using BPN.ECommerce.Application.Common;
using BPN.ECommerce.Application.Orders.Exceptions;
using BPN.ECommerce.Application.Orders.Inputs;
using BPN.ECommerce.Application.Orders.Mapper;
using BPN.ECommerce.Application.Products.Exceptions;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Application.Services.Redis;
using BPN.ECommerce.Domain.Aggregates.Orders.Entities;
using BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BPN.ECommerce.Application.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommand(CreateOrderInput input) : IRequest
{
    public CreateOrderInput Input { get; set; } = input;

    public static CreateOrderCommand Create(CreateOrderInput input)
    {
        return new CreateOrderCommand(input);
    }
}

public sealed class CreateOrderCommandHandler(
    IBalanceServiceClient balanceServiceClient,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IOrderMapper orderMapper,
    ILogger<CreateOrderCommandHandler> logger,
    IRedisServiceClient redisServiceClient)
    : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        using var senderLock =
            await redisServiceClient.AcquireLockAsync(request.Input.OrderId).ConfigureAwait(false);
       
        if (senderLock is null)
        {
            throw new LockException("Failed to acquire lock");
        }
        
        var orderItems = new List<OrderItem>();
        var orderId = request.Input.OrderId;
        decimal totalAmount = decimal.Zero;
        
        foreach (var orderLine in request.Input.Items)
        {
            var products = await balanceServiceClient.GetProducts();
            
            var selectedProduct = products.Data.FirstOrDefault(p => p.Id == orderLine.ProductId);
            
            if (selectedProduct == null || selectedProduct.Stock < orderLine.Quantity)
                throw new ProductNotFoundException("Product not found or stock is invalid");
            
            orderItems.Add(OrderItem.Create(orderId,selectedProduct.Id,orderLine.Quantity, selectedProduct.Price));
          
            totalAmount += orderLine.Quantity * selectedProduct.Price;
        }

        var order = Order.Create(orderId, orderItems, totalAmount, OrderStatus.Pending());

        await orderRepository.AddAsync(order, cancellationToken);

        var initPaymentResponse = await balanceServiceClient.InitPayment(
            orderMapper.MapToInitPaymentRequest(orderId, totalAmount), cancellationToken);
        
        if (!initPaymentResponse.Success || initPaymentResponse.Data?.PreOrder == null)
        {
            logger.LogError("Failed to reserve funds for OrderId {OrderId}: {Message}", orderId, initPaymentResponse.Message);

            throw new InitPaymentException(initPaymentResponse.Message);
        }

        if (initPaymentResponse.Data.UpdatedBalance.AvailableBalance < totalAmount)
        {
            logger.LogWarning(
                "Not enough balance for OrderId {OrderId}. Required: {Required}, Available: {Available}",
                orderId, totalAmount, initPaymentResponse.Data.UpdatedBalance.AvailableBalance);

            throw new BalanceException(!string.IsNullOrEmpty(initPaymentResponse.Message)
                ? initPaymentResponse.Message
                : "Insufficient balance");
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}