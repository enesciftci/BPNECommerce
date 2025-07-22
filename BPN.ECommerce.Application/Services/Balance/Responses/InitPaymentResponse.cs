namespace BPN.ECommerce.Application.Services.Balance.Responses;

public class InitPaymentResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public InitPaymentData Data { get; set; }
}

public class InitPaymentData
{
    public PreOrder PreOrder { get; set; }
    public UpdatedBalance UpdatedBalance { get; set; }
}

public class PreOrder
{
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
}

