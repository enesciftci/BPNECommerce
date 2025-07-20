using System.Net.Http.Json;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Infrastructure.Services.Balance.Mapper;
using BPN.ECommerce.Infrastructure.Services.Balance.Responses;

namespace BPN.ECommerce.Infrastructure.Services.Balance;

public class BalanceServiceClient : IBalanceServiceClient
{
    private readonly HttpClient _client;
    private readonly IBalanceServiceMapper _mapper;

    public BalanceServiceClient(HttpClient client, IBalanceServiceMapper mapper)
    {
        this._client = client;
        _mapper = mapper;
    }

    public async Task<GetProductsResponse> GetProducts()
    {
        var response = await _client.GetFromJsonAsync<GetProductsServiceResponse>("/api/products");
        
        return _mapper.MapToGetProductsResponse(response);
    }
}