namespace BPN.ECommerce.Infrastructure.Services.Balance.Responses;

public class VoidPaymentServiceResponse
{
    public string Error { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public VoidPaymentData Data { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class VoidPaymentData
{
    public Order Order { get; set; }
    public UpdatedBalance UpdatedBalance { get; set; }
}

public class Root
{
  
}

