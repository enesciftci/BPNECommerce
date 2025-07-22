namespace BPN.ECommerce.Infrastructure.Services.Balance.Responses;

public class UpdatedBalance
{
    public string UserId { get; set; }
    public long TotalBalance { get; set; }
    public long AvailableBalance { get; set; }
    public int BlockedBalance { get; set; }
    public string Currency { get; set; }
    public DateTime LastUpdated { get; set; }
}