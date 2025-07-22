namespace BPN.ECommerce.Infrastructure.Services.Balance.Responses;

public class AuthPaymentServiceResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public AuthPaymentData Data { get; set; }
    public string Error { get; set; }
}

public class AuthPaymentData
{
    public Order Order { get; set; }
    public UpdatedBalance UpdatedBalance { get; set; }
}

