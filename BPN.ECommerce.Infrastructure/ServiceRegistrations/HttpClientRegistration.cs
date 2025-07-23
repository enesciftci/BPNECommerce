using System.Reflection;
using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Infrastructure.Services.Balance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace BPN.ECommerce.Infrastructure.ServiceRegistrations;

public static class HttpClientRegistration
{
    public static void RegisterHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IBalanceServiceClient, BalanceServiceClient>(p =>
            {
                var balanceApiConfig = configuration.GetSection("BalanceApiConfig");
                p.BaseAddress = new Uri(balanceApiConfig.GetValue<string>("BaseUrl"));
                p.Timeout = TimeSpan.FromMilliseconds(balanceApiConfig.GetValue<int>("Timeout"));
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(provider =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<CircuitBreakerPolicyFactory>>();
                var factory = new CircuitBreakerPolicyFactory(logger);
                return factory.Create();
            })
            .AddPolicyHandler(GetRetryPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(2), retryCount: 3);
        return HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(delay);
    }

   
}