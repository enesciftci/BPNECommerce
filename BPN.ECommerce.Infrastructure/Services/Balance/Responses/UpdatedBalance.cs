namespace BPN.ECommerce.Infrastructure.Services.Balance.Responses;

public class UpdatedBalance
{
    public string UserId { get; set; }
    public decimal TotalBalance { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal BlockedBalance { get; set; }
    public string Currency { get; set; }
    public DateTime LastUpdated { get; set; }
}