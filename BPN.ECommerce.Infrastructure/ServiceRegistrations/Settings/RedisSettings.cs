namespace BPN.ECommerce.Infrastructure.ServiceRegistrations.Settings;

public class RedisSettings
{
    public string BaseAddress { get; set; }
    public int Port { get; set; }
    public int Timeout { get; set; }
    public int Expiry { get; set; }
    public int Wait { get; set; }
    public int RetryCount { get; set; }
    public int RetryDelay { get; set; }
}