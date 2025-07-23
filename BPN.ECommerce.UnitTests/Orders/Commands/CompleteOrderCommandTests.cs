using BPN.ECommerce.Application;
using BPN.ECommerce.Application.Common;
using BPN.ECommerce.Application.Orders;
using BPN.ECommerce.Application.Orders.Notifications.VoidPayment;
using BPN.ECommerce.Application.Services.Balance.Requests;
using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Domain.Aggregates.Orders.Entities;
using Order = BPN.ECommerce.Domain.Aggregates.Orders.Entities.Order;
using BPN.ECommerce.Application.Orders.Commands.CompleteOrder;
using BPN.ECommerce.Application.Orders.Exceptions;
using BPN.ECommerce.Application.Orders.Inputs;
using BPN.ECommerce.Application.Orders.Mapper;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Application.Services.Redis;
using BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace BPN.ECommerce.UnitTests.Orders.Commands;

[TestFixture]
public class CompleteOrderCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IBalanceServiceClient> _balanceServiceMock;
    private Mock<IOrderMapper> _orderMapperMock;
    private Mock<IOrderRepository> _orderRepoMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<ILogger<CompleteOrderCommandHandler>> _loggerMock;
    private Mock<IRedisServiceClient> _redisMock = null!;
    
    private CompleteOrderCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _balanceServiceMock = new Mock<IBalanceServiceClient>();
        _orderMapperMock = new Mock<IOrderMapper>();
        _orderRepoMock = new Mock<IOrderRepository>();
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<CompleteOrderCommandHandler>>();
        _redisMock = new Mock<IRedisServiceClient>();

        _handler = new CompleteOrderCommandHandler(
            _unitOfWorkMock.Object,
            _balanceServiceMock.Object,
            _orderMapperMock.Object,
            _orderRepoMock.Object,
            _mediatorMock.Object,
            _loggerMock.Object,
            _redisMock.Object
        );
    }

    [Test]
    public void Handle_PreOrderNotFound_ThrowsException()
    {
        // Arrange
        var orderId = Guid.NewGuid().ToString();
        var input = new CompleteOrderInput { OrderId = orderId };
        var command = new CompleteOrderCommand(input);

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        _orderRepoMock.Setup(r => r.GetByOrderId(orderId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Order)null!);

        // Act & Assert
        Assert.ThrowsAsync<PreOrderNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Handle_AuthPaymentFails_ThrowsAuthPaymentException_AndPublishesEvent()
    {
        // Arrange
        var orderId = Guid.NewGuid().ToString();
        var input = new CompleteOrderInput { OrderId = orderId };
        var command = new CompleteOrderCommand(input);

        var fakeOrder = Order.Create(orderId, new List<OrderItem>(), 100, OrderStatus.Pending());

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        
        _orderRepoMock.Setup(r => r.GetByOrderId(orderId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(fakeOrder);

        _orderMapperMock.Setup(m => m.MapToAuthPaymentRequest(orderId))
                        .Returns(new AuthPaymentRequest());

        _balanceServiceMock.Setup(b => b.AuthPayment(It.IsAny<AuthPaymentRequest>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new AuthPaymentResponse()
                           {
                               Success = false,
                               Message = "Yetersiz bakiye"
                           });

        _orderMapperMock.Setup(m => m.MapToVoidPaymentNotification(orderId))
                        .Returns(new Mock<VoidPaymentNotification>().Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<AuthPaymentException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.That(ex, Is.TypeOf<AuthPaymentException>());

        _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_AuthPaymentSuccess_UpdatesOrder_AndSavesChanges()
    {
        // Arrange
        var orderId = Guid.NewGuid().ToString();
        var input = new CompleteOrderInput { OrderId = orderId };
        var command = new CompleteOrderCommand(input);

        var fakeOrder = Order.Create(orderId, new List<OrderItem>(), 100, OrderStatus.Approved());

        var completedAt = DateTime.UtcNow;

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        _orderRepoMock.Setup(r => r.GetByOrderId(orderId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(fakeOrder);

        _orderMapperMock.Setup(m => m.MapToAuthPaymentRequest(orderId))
                        .Returns(new AuthPaymentRequest());

        _balanceServiceMock.Setup(b => b.AuthPayment(It.IsAny<AuthPaymentRequest>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new AuthPaymentResponse()
                           {
                               Success = true,
                               Data = new AuthPaymentData()
                               {
                                   Order = new Application.Services.Balance.Responses.Order() { CompletedAt = completedAt }
                               }
                           });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(fakeOrder.Status, Is.EqualTo(OrderStatus.Approved()));
        Assert.That(fakeOrder.CompletedAt, Is.EqualTo(completedAt));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public void Handle_Should_Throw_When_Lock_Not_Acquired()
    {
        // Arrange
        var input = new CompleteOrderInput()
        {
            OrderId = "order-lock-fail"
        };

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync((IDisposable?)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<LockException>(() =>
            _handler.Handle(CompleteOrderCommand.Create(input), CancellationToken.None));
        Assert.That(ex!.Message, Is.EqualTo("Failed to acquire lock"));
    }
}
