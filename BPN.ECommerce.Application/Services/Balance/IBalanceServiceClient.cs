using BPN.ECommerce.Application.Services.Balance.Requests;
using BPN.ECommerce.Application.Services.Balance.Responses;

namespace BPN.ECommerce.Application.Services.Balance;

public interface IBalanceServiceClient
{
    Task<GetProductsResponse> GetProducts();
    Task<InitPaymentResponse> InitPayment(InitPaymentRequest request, CancellationToken cancellationToken);
    Task<AuthPaymentResponse> AuthPayment(AuthPaymentRequest request, CancellationToken cancellationToken);
    Task<VoidPaymentResponse> VoidPayment(VoidPaymentRequest request, CancellationToken cancellationToken);
}