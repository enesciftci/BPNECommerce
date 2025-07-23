using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Application.Services.Balance.Requests;
using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Infrastructure.Services.Balance.Mapper;
using BPN.ECommerce.Infrastructure.Services.Balance.Responses;

namespace BPN.ECommerce.Infrastructure.Services.Balance;

public class BalanceServiceClient: IBalanceServiceClient
{
    private readonly HttpClient client;
    private readonly IBalanceServiceMapper mapper;
    private readonly JsonSerializerOptions _jsonOptions;
    public BalanceServiceClient(HttpClient client, IBalanceServiceMapper mapper)
    {
        this.client = client;
        this.mapper = mapper;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };
    }
   

    public async Task<GetProductsResponse> GetProducts()
    {
        var response = await client.GetFromJsonAsync<GetProductsServiceResponse>("/api/products");
        
        return mapper.MapToGetProductsResponse(response);
    }

    public async Task<InitPaymentResponse> InitPayment(InitPaymentRequest request, CancellationToken cancellationToken)
    {
        const string url = "/api/balance/preorder";
        
        var response = await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(request, _jsonOptions),
            Encoding.UTF8,"application/json"), cancellationToken);
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        
        return mapper.MapToInitPaymentResponse(
            JsonSerializer.Deserialize<InitPaymentServiceResponse>(responseContent, _jsonOptions)!);
    }

    public async Task<AuthPaymentResponse> AuthPayment(AuthPaymentRequest request, CancellationToken cancellationToken)
    {
        const string url = "/api/balance/complete";

        var response = await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(request, _jsonOptions),
            Encoding.UTF8,"application/json"), cancellationToken);
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        return mapper.MapToAuthPaymentResponse(JsonSerializer.Deserialize<AuthPaymentServiceResponse>(responseContent, _jsonOptions)!);
    }

    public async Task<VoidPaymentResponse> VoidPayment(VoidPaymentRequest request, CancellationToken cancellationToken)
    {
        const string url = "/api/balance/cancel";

        var response = await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(request, _jsonOptions),
            Encoding.UTF8,"application/json"), cancellationToken);
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        return mapper.MapToVoidPaymentResponse(JsonSerializer.Deserialize<VoidPaymentServiceResponse>(responseContent, _jsonOptions)!);
    }
}