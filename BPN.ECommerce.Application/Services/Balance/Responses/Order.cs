namespace BPN.ECommerce.Application.Services.Balance.Responses;

public class Order
{
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}