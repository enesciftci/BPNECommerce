using System.Text.Json.Serialization;

namespace BPN.ECommerce.Application.Services.Balance.Requests;

public class AuthPaymentRequest
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; }
}