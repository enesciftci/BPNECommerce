using BPN.ECommerce.Application.Services.Balance;
using BPN.ECommerce.Infrastructure.Services.Balance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }).SetHandlerLifetime(TimeSpan.FromMinutes(5));
    }
}