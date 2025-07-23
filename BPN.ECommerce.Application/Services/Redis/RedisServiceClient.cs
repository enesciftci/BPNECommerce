namespace BPN.ECommerce.Application.Services.Redis;

public interface IRedisServiceClient
{
    Task<IDisposable?> AcquireLockAsync(string key);
}