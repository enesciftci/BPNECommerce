using BPN.ECommerce.Application.Services.Balance.Responses;

namespace BPN.ECommerce.Application.Services.Balance;

public interface IBalanceServiceClient
{
    Task<GetProductsResponse> GetProducts();
}