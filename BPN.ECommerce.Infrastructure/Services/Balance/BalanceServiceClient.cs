using System.Net.Http.Json;
using System.Text.Json;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Application.Services.Balance.Requests;
using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Infrastructure.Services.Balance.Mapper;
using BPN.ECommerce.Infrastructure.Services.Balance.Responses;

namespace BPN.ECommerce.Infrastructure.Services.Balance;

public class BalanceServiceClient(HttpClient client, IBalanceServiceMapper mapper) : IBalanceServiceClient
{
    public async Task<GetProductsResponse> GetProducts()
    {
        var response = await client.GetFromJsonAsync<GetProductsServiceResponse>("/api/products");
        
        return mapper.MapToGetProductsResponse(response);
    }

    public async Task<InitPaymentResponse> InitPayment(InitPaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await client.PostAsync("/api/balance/preorder", new StringContent(JsonSerializer.Serialize(request)), cancellationToken);
        
        var result = await response.Content.ReadFromJsonAsync<InitPaymentServiceResponse>(cancellationToken);
        
        return mapper.MapToInitPaymentResponse(result);
    }

    public async Task<AuthPaymentResponse> AuthPayment(AuthPaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await client.PostAsync("/api/balance/complete", new StringContent(JsonSerializer.Serialize(request)), cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<AuthPaymentServiceResponse>(cancellationToken);
        
        return mapper.MapToAuthPaymentResponse(result);
    }

    public async Task<VoidPaymentResponse> VoidPayment(VoidPaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await client.PostAsync("/api/balance/cancel", new StringContent(JsonSerializer.Serialize(request)), cancellationToken);
        
        var result = await response.Content.ReadFromJsonAsync<VoidPaymentServiceResponse>(cancellationToken);
        
        return mapper.MapToVoidPaymentResponse(result);
    }
}