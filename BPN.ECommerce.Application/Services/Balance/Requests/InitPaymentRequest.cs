using System.Text.Json.Serialization;

namespace BPN.ECommerce.Application.Services.Balance.Requests;

public class InitPaymentRequest
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; }
}