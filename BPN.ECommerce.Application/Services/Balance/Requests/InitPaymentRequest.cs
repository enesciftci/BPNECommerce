namespace BPN.ECommerce.Application.Services.Balance.Requests;

public class InitPaymentRequest
{
    public int Amount { get; set; }
    public string OrderId { get; set; }
}