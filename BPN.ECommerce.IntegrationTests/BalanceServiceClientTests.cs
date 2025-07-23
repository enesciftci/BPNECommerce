using System.Net;
using System.Text;
using System.Text.Json;
using BPN.ECommerce.Application.Services.Balance.Requests;
using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Infrastructure.Services.Balance;
using BPN.ECommerce.Infrastructure.Services.Balance.Mapper;
using BPN.ECommerce.Infrastructure.Services.Balance.Responses;
using FluentAssertions;
using Moq;
using InitPaymentData = BPN.ECommerce.Infrastructure.Services.Balance.Responses.InitPaymentData;

namespace BPN.ECommerce.IntegrationTests;

[TestFixture]
public class BalanceServiceClientTests
{
    private static HttpClient CreateHttpClient(HttpResponseMessage fakeResponse)
    {
        var handler = new FakeHttpMessageHandler(fakeResponse);
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://fake-url.com")
        };
    }

    [Test]
    public async Task GetProducts_ShouldReturnMappedResult()
    {
        // Arrange
        var serviceResponse = new GetProductsServiceResponse { Data = new List<ProductData> {new ProductData()
        {
            Category = "phone",
            Description = "iphone",
            Id = "1231",
            Name = "iphone",
            Currency = "TRY",
            Price = 100,
            Stock = 1
        } } };
        var json = JsonSerializer.Serialize(serviceResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var mockMapper = new Mock<IBalanceServiceMapper>();
        mockMapper.Setup(x => x.MapToGetProductsResponse(It.IsAny<GetProductsServiceResponse>()))
            .Returns(new GetProductsResponse { Data = new List<ProductDto>(){new ProductDto()
            {
                Category = "phone",
                Description = "iphone",
                Id = "1231",
                Name = "iphone",
                Currency = "TRY",
                Price = 100,
                Stock = 1 
            }}});

        var client = new BalanceServiceClient(CreateHttpClient(httpResponse), mockMapper.Object);

        // Act
        var result = await client.GetProducts();

        // Assert
        result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Test]
    public async Task InitPayment_ShouldReturnMappedResult()
    {
        var serviceResponse = new InitPaymentServiceResponse { Data = new InitPaymentData() };
        var json = JsonSerializer.Serialize(serviceResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var mockMapper = new Mock<IBalanceServiceMapper>();
        mockMapper.Setup(x => x.MapToInitPaymentResponse(It.IsAny<InitPaymentServiceResponse>()))
            .Returns(new InitPaymentResponse { Data = new Application.Services.Balance.Responses.InitPaymentData(),Success = true});

        var client = new BalanceServiceClient(CreateHttpClient(httpResponse), mockMapper.Object);

        var result = await client.InitPayment(new InitPaymentRequest(), CancellationToken.None);

        result.Success.Should().Be(true);
    }

    [Test]
    public async Task AuthPayment_ShouldReturnMappedResult()
    {
        var serviceResponse = new AuthPaymentServiceResponse { Success = true };
        var json = JsonSerializer.Serialize(serviceResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var mockMapper = new Mock<IBalanceServiceMapper>();
        mockMapper.Setup(x => x.MapToAuthPaymentResponse(It.IsAny<AuthPaymentServiceResponse>()))
            .Returns(new AuthPaymentResponse { Success = true});

        var client = new BalanceServiceClient(CreateHttpClient(httpResponse), mockMapper.Object);

        var result = await client.AuthPayment(new AuthPaymentRequest(), CancellationToken.None);

        result.Success.Should().Be(true);
    }

    [Test]
    public async Task VoidPayment_ShouldReturnMappedResult()
    {
        var serviceResponse = new VoidPaymentServiceResponse { Success = true };
        var json = JsonSerializer.Serialize(serviceResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var mockMapper = new Mock<IBalanceServiceMapper>();
        mockMapper.Setup(x => x.MapToVoidPaymentResponse(It.IsAny<VoidPaymentServiceResponse>()))
            .Returns(new VoidPaymentResponse { Success = true });

        var client = new BalanceServiceClient(CreateHttpClient(httpResponse), mockMapper.Object);

        var result = await client.VoidPayment(new VoidPaymentRequest(), CancellationToken.None);

        result.Success.Should().BeTrue();
    }
}
