namespace BPN.ECommerce.Infrastructure.Services.Balance.Requests;

public class InitPaymentServiceRequest
{
    public int Amount { get; set; }
    public string OrderId { get; set; }
}