namespace BPN.ECommerce.Infrastructure.Services.Balance.Requests;

public class InitPaymentServiceRequest
{
    public decimal Amount { get; set; }
    public string OrderId { get; set; }
}