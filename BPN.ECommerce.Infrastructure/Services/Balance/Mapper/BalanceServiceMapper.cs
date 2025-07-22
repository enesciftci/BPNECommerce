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
        return new InitPaymentResponse()
        {
            Success = response.Success,
            Message = response.Message,
            Data = new InitPaymentData()
            {
                PreOrder = new PreOrder()
                {
                    Amount = Money.Create(response.Data.PreOrder.Amount).Amount,
                    Status = response.Data.PreOrder.Status,
                    Timestamp = response.Data.PreOrder.Timestamp,
                    OrderId = response.Data.PreOrder.OrderId,
                },
                UpdatedBalance = new UpdatedBalance()
                {
                    Currency = response.Data.UpdatedBalance.Currency,
                    AvailableBalance = Money.Create(response.Data.UpdatedBalance.AvailableBalance).Amount,
                    BlockedBalance = Money.Create(response.Data.UpdatedBalance.BlockedBalance).Amount,
                    LastUpdated = response.Data.UpdatedBalance.LastUpdated,
                    TotalBalance = Money.Create(response.Data.UpdatedBalance.TotalBalance).Amount,
                    UserId = response.Data.UpdatedBalance.UserId
                }
            }
        };
    }

    public AuthPaymentResponse MapToAuthPaymentResponse(AuthPaymentServiceResponse response)
    {
        return new AuthPaymentResponse()
        {
            Success = response.Success,
            Message = response.Message,
            Error = response.Error,
            Data = new AuthPaymentData()
            {
                Order = new Order()
                {
                    Amount = Money.Create(response.Data.Order.Amount).Amount,
                    Status = response.Data.Order.Status,
                    Timestamp = response.Data.Order.Timestamp,
                    OrderId = response.Data.Order.OrderId,
                    CompletedAt = response.Data.Order.CompletedAt.Value,
                },
                UpdatedBalance = new UpdatedBalance()
                {
                    Currency = response.Data.UpdatedBalance.Currency,
                    AvailableBalance = Money.Create(response.Data.UpdatedBalance.AvailableBalance).Amount,
                    BlockedBalance = Money.Create(response.Data.UpdatedBalance.BlockedBalance).Amount,
                    LastUpdated = response.Data.UpdatedBalance.LastUpdated,
                    TotalBalance = Money.Create(response.Data.UpdatedBalance.TotalBalance).Amount,
                    UserId = response.Data.UpdatedBalance.UserId
                }
            }
        };
    }
    
    public VoidPaymentResponse MapToVoidPaymentResponse(VoidPaymentServiceResponse response)
    {
        return new VoidPaymentResponse()
        {
            Success = response.Success,
            Message = response.Message,
            Error = response.Error,
            Data = new VoidPaymentData()
            {
                Order = new Order()
                {
                    Amount = Money.Create(response.Data.Order.Amount).Amount,
                    Status = response.Data.Order.Status,
                    Timestamp = response.Data.Order.Timestamp,
                    OrderId = response.Data.Order.OrderId,
                    CancelledAt = response.Data.Order.CancelledAt.Value,
                },
                UpdatedBalance = new UpdatedBalance()
                {
                    Currency = response.Data.UpdatedBalance.Currency,
                    AvailableBalance = Money.Create(response.Data.UpdatedBalance.AvailableBalance).Amount,
                    BlockedBalance = Money.Create(response.Data.UpdatedBalance.BlockedBalance).Amount,
                    LastUpdated = response.Data.UpdatedBalance.LastUpdated,
                    TotalBalance = Money.Create(response.Data.UpdatedBalance.TotalBalance).Amount,
                    UserId = response.Data.UpdatedBalance.UserId
                }
            }
        };
    }
}