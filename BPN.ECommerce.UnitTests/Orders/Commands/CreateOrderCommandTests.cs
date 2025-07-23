using BPN.ECommerce.Application;
using BPN.ECommerce.Application.Common;
using BPN.ECommerce.Application.Orders;
using BPN.ECommerce.Application.Orders.Commands.CreateOrder;
using BPN.ECommerce.Application.Orders.Exceptions;
using BPN.ECommerce.Application.Orders.Inputs;
using BPN.ECommerce.Application.Orders.Mapper;
using BPN.ECommerce.Application.Products.Exceptions;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Application.Services.Balance.Requests;
using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Application.Services.Redis;
using Order = BPN.ECommerce.Domain.Aggregates.Orders.Entities.Order;
using Microsoft.Extensions.Logging;
using Moq;

namespace BPN.ECommerce.UnitTests.Orders.Commands;

[TestFixture]
public class CreateOrderCommandHandlerTests
{
    private Mock<IBalanceServiceClient> _balanceServiceMock = null!;
    private Mock<IOrderRepository> _orderRepositoryMock = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IOrderMapper> _orderMapperMock = null!;
    private Mock<ILogger<CreateOrderCommandHandler>> _loggerMock = null!;
    private Mock<IRedisServiceClient> _redisMock = null!;
    private CreateOrderCommandHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _balanceServiceMock = new Mock<IBalanceServiceClient>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderMapperMock = new Mock<IOrderMapper>();
        _loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
        _redisMock = new Mock<IRedisServiceClient>();

        _handler = new CreateOrderCommandHandler(
            _balanceServiceMock.Object,
            _orderRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _orderMapperMock.Object,
            _loggerMock.Object,
            _redisMock.Object
        );
    }

    [Test]
    public async Task Handle_Should_CreateOrder_Successfully()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderId = "test-123",
            Items = new List<OrderLine>
            {
                new("p1", 2)
            }
        };

        var products = new List<ProductDto>
        {
            new() { Id = "p1", Stock = 10, Price = 5 }
        };

        var paymentResponse = new InitPaymentResponse()
        {
            Success = true,
            Data = new()
            {
                PreOrder = new PreOrder(),
                UpdatedBalance = new UpdatedBalance() { AvailableBalance = 100 }
            }
        };

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        
        _balanceServiceMock.Setup(x => x.GetProducts())
            .ReturnsAsync(new GetProductsResponse() { Data = products });

        _balanceServiceMock.Setup(x => x.InitPayment(It.IsAny<InitPaymentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentResponse);

        _orderMapperMock.Setup(x => x.MapToInitPaymentRequest(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(new InitPaymentRequest());

// Act
        await _handler.Handle(CreateOrderCommand.Create(input), CancellationToken.None);

// Assert
        _orderRepositoryMock.Verify(x => x.AddAsync(
            It.Is<Order>(o =>
                o.OrderId == input.OrderId &&
                o.Amount == 10 &&
                o.Items.Count == 1 &&
                o.Items.First().ProductId == "p1" &&
                o.Items.First().Quantity == 2
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_Should_Throw_When_Product_Not_Found()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderId = "test-123",
            Items = new List<OrderLine>
            {
                new("not-found", 1)
            }
        };

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        _balanceServiceMock.Setup(x => x.GetProducts())
            .ReturnsAsync(new GetProductsResponse() { Data = new List<ProductDto>() });

        // Act & Assert
        Assert.ThrowsAsync<ProductNotFoundException>(() =>
            _handler.Handle(CreateOrderCommand.Create(input), CancellationToken.None));
    }

    [Test]
    public void Handle_Should_Throw_When_InitPayment_Fails()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderId = "test-123",
            Items = new List<OrderLine>
            {
                new("p1", 1)
            }
        };

        var products = new List<ProductDto>
        {
            new() { Id = "p1", Stock = 10, Price = 5 }
        };

        _balanceServiceMock.Setup(x => x.GetProducts())
            .ReturnsAsync(new GetProductsResponse()
            {
                Data = products
            });

        _balanceServiceMock.Setup(x => x.InitPayment(It.IsAny<InitPaymentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new InitPaymentResponse()
            {
                Success = false,
                Message = "init failed"
            });

        _orderMapperMock.Setup(x => x.MapToInitPaymentRequest(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(new InitPaymentRequest());

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        // Act & Assert
        var ex = Assert.ThrowsAsync<InitPaymentException>(() =>
            _handler.Handle(CreateOrderCommand.Create(input), CancellationToken.None));
        Assert.That(ex!.Message, Is.EqualTo("init failed"));
    }

    [Test]
    public void Handle_Should_Throw_When_Insufficient_Balance()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderId = "test-123",
            Items = new List<OrderLine>
            {
                new("p1", 2)
            }
        };

        var products = new List<ProductDto>
        {
            new() { Id = "p1", Stock = 10, Price = 50 }
        };

        var response = new InitPaymentResponse()
        {
            Success = true,
            Data = new()
            {
                PreOrder = new PreOrder(),
                UpdatedBalance = new UpdatedBalance() { AvailableBalance = 50 } // should be 100
            }
        };

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        _balanceServiceMock.Setup(x => x.GetProducts()).ReturnsAsync(new GetProductsResponse() { Data = products });
        _balanceServiceMock.Setup(x => x.InitPayment(It.IsAny<InitPaymentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        _orderMapperMock.Setup(x => x.MapToInitPaymentRequest(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(new InitPaymentRequest());

        // Act & Assert
        Assert.ThrowsAsync<BalanceException>(() =>
            _handler.Handle(CreateOrderCommand.Create(input), CancellationToken.None));
    }
    
    [Test]
    public void Handle_Should_Throw_When_Lock_Not_Acquired()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderId = "order-lock-fail",
            Items = new List<OrderLine>
            {
                new("p1", 1)
            }
        };

        _redisMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>()))
            .ReturnsAsync((IDisposable?)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<LockException>(() =>
            _handler.Handle(CreateOrderCommand.Create(input), CancellationToken.None));
        Assert.That(ex!.Message, Is.EqualTo("Failed to acquire lock"));
    }
}