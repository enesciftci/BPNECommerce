using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;
using BPN.ECommerce.Infrastructure.Services.Balance.Responses;
using AuthPaymentData = BPN.ECommerce.Application.Services.Balance.Responses.AuthPaymentData;
using InitPaymentData = BPN.ECommerce.Application.Services.Balance.Responses.InitPaymentData;
using Order = BPN.ECommerce.Application.Services.Balance.Responses.Order;
using PreOrder = BPN.ECommerce.Application.Services.Balance.Responses.PreOrder;
using UpdatedBalance = BPN.ECommerce.Application.Services.Balance.Responses.UpdatedBalance;
using VoidPaymentData = BPN.ECommerce.Application.Services.Balance.Responses.VoidPaymentData;

namespace BPN.ECommerce.Infrastructure.Services.Balance.Mapper;

public interface IBalanceServiceMapper
{
    GetProductsResponse MapToGetProductsResponse(GetProductsServiceResponse response);
    InitPaymentResponse MapToInitPaymentResponse(InitPaymentServiceResponse response);
    AuthPaymentResponse MapToAuthPaymentResponse(AuthPaymentServiceResponse response);
    VoidPaymentResponse MapToVoidPaymentResponse(VoidPaymentServiceResponse response);
}
public class BalanceServiceMapper : IBalanceServiceMapper
{
    public GetProductsResponse MapToGetProductsResponse(GetProductsServiceResponse response)
    {
        if (response?.Data == null)
        {
            return new GetProductsResponse();
        }

        return new GetProductsResponse()
        {
            Success = response.Success,
            Data = response.Data.Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category,
                Currency = p.Currency,
                Stock = p.Stock
            }).ToList()
        };
    }

    public InitPaymentResponse MapToInitPaymentResponse(InitPaymentServiceResponse response)
    {
        var initPaymentResponse = new InitPaymentResponse()
        {
            Success = response.Success,
            Message = response.Message,
        };

        if (response.Data != null)
        {
            initPaymentResponse.Data = new InitPaymentData()
            {
                PreOrder = new PreOrder()
                {
                    Amount = response.Data.PreOrder.Amount,
                    Status = response.Data.PreOrder.Status,
                    Timestamp = response.Data.PreOrder.Timestamp,
                    OrderId = response.Data.PreOrder.OrderId,
                },
                UpdatedBalance = new UpdatedBalance()
                {
                    Currency = response.Data.UpdatedBalance.Currency,
                    AvailableBalance = response.Data.UpdatedBalance.AvailableBalance,
                    BlockedBalance = response.Data.UpdatedBalance.BlockedBalance,
                    LastUpdated = response.Data.UpdatedBalance.LastUpdated,
                    TotalBalance = response.Data.UpdatedBalance.TotalBalance,
                    UserId = response.Data.UpdatedBalance.UserId
                }
            };
        }
        
        return initPaymentResponse;
    }

    public AuthPaymentResponse MapToAuthPaymentResponse(AuthPaymentServiceResponse response)
    {
        var authPaymentResponse = new AuthPaymentResponse()
        {
            Success = response.Success,
            Message = response.Message,
            Error = response.Error,
        };

        if (response.Data != null)
        {
            authPaymentResponse.Data = new AuthPaymentData()
            {
                Order = new Order()
                {
                    Amount = response.Data.Order.Amount,
                    Status = response.Data.Order.Status,
                    Timestamp = response.Data.Order.Timestamp,
                    OrderId = response.Data.Order.OrderId,
                    CompletedAt = response.Data.Order.CompletedAt.Value,
                },
                UpdatedBalance = new UpdatedBalance()
                {
                    Currency = response.Data.UpdatedBalance.Currency,
                    AvailableBalance = response.Data.UpdatedBalance.AvailableBalance,
                    BlockedBalance = response.Data.UpdatedBalance.BlockedBalance,
                    LastUpdated = response.Data.UpdatedBalance.LastUpdated,
                    TotalBalance = response.Data.UpdatedBalance.TotalBalance,
                    UserId = response.Data.UpdatedBalance.UserId
                }
            };
        }

        return authPaymentResponse;
    }

    public VoidPaymentResponse MapToVoidPaymentResponse(VoidPaymentServiceResponse response)
    {
        var voidPaymentResponse = new VoidPaymentResponse()
        {
            Success = response.Success,
            Message = response.Message,
            Error = response.Error,
        };
        if (response.Data != null)
        {
            voidPaymentResponse.Data = new VoidPaymentData()
            {
                Order = new Order()
                {
                    Amount = response.Data.Order.Amount,
                    Status = response.Data.Order.Status,
                    Timestamp = response.Data.Order.Timestamp,
                    OrderId = response.Data.Order.OrderId,
                    CancelledAt = response.Data.Order.CancelledAt.Value,
                },
                UpdatedBalance = new UpdatedBalance()
                {
                    Currency = response.Data.UpdatedBalance.Currency,
                    AvailableBalance = response.Data.UpdatedBalance.AvailableBalance,
                    BlockedBalance = response.Data.UpdatedBalance.BlockedBalance,
                    LastUpdated = response.Data.UpdatedBalance.LastUpdated,
                    TotalBalance = response.Data.UpdatedBalance.TotalBalance,
                    UserId = response.Data.UpdatedBalance.UserId
                }
            };
        }
        return voidPaymentResponse;
    }
}