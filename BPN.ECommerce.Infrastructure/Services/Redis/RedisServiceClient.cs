using BPN.ECommerce.Application.Services.Redis;
using BPN.ECommerce.Infrastructure.ServiceRegistrations.Settings;
using RedLockNet;

namespace BPN.ECommerce.Infrastructure.Services.Redis;


public class RedisServiceClient : IRedisServiceClient
{
    private readonly RedisSettings redisOptions;
    private readonly IDistributedLockFactory redlockFactory;

    public RedisServiceClient(IDistributedLockFactory redlockFactory, RedisSettings redisOptions)
    {
        this.redlockFactory = redlockFactory;
        this.redisOptions = redisOptions;
    }

    public async Task<IDisposable?> AcquireLockAsync(string key)
    {
        for (int retry = 0; retry < redisOptions.RetryCount; retry++)
        {
            Console.WriteLine($"Retry {retry + 1} of {redisOptions.RetryCount}");

            var redlock = await redlockFactory.CreateLockAsync(
                resource: key,
                expiryTime: TimeSpan.FromSeconds(redisOptions.Expiry),
                waitTime: TimeSpan.FromSeconds(redisOptions.Wait),
                retryTime: TimeSpan.FromMilliseconds(redisOptions.RetryDelay)
            ).ConfigureAwait(false);

            if (redlock.IsAcquired)
            {
                return redlock;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(redisOptions.RetryDelay)).ConfigureAwait(false);
        }

        return null;
    }
}