using System.Text.Json.Serialization;

namespace BPN.ECommerce.Application.Services.Balance.Requests;

public class VoidPaymentRequest
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; }
}