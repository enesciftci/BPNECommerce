namespace BPN.ECommerce.Application.Services.Balance.Responses;

public class VoidPaymentResponse
{
    public string Error { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public VoidPaymentData Data { get; set; }
}

public class VoidPaymentData
{
    public Order Order { get; set; }
    public UpdatedBalance UpdatedBalance { get; set; }
}