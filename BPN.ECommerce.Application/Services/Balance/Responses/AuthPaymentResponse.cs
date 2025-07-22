namespace BPN.ECommerce.Application.Services.Balance.Responses;

public class AuthPaymentResponse
{
    public string Error { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public AuthPaymentData Data { get; set; }
    
}
public class AuthPaymentData
{
    public Order Order { get; set; }
    public UpdatedBalance UpdatedBalance { get; set; }
}