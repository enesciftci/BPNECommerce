using System.Net;
using System.Reflection;
using BPN.ECommerce.Application;
using BPN.ECommerce.Application.Orders;
using BPN.ECommerce.Application.Orders.Inputs;
using BPN.ECommerce.Application.Orders.Mapper;
using BPN.ECommerce.Application.Products.Mapper;
using BPN.ECommerce.Application.Services.Redis;
using BPN.ECommerce.Infrastructure.Persistence.EFCore;
using BPN.ECommerce.Infrastructure.Persistence.EFCore.Orders.EntityConfigurations;
using BPN.ECommerce.Infrastructure.ServiceRegistrations.Settings;
using BPN.ECommerce.Infrastructure.Services.Balance.Mapper;
using BPN.ECommerce.Infrastructure.Services.Redis;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace BPN.ECommerce.Infrastructure.ServiceRegistrations;

public static class Registrations
{
    public static void RegisterServices(this IServiceCollection services)
    {
        AddMediatR(services);
        AddApiMappers(services);
        AddClientMappers(services);
        AddRepositories(services);
        AddValidators(services);
    }
    private static void AddApiMappers(IServiceCollection services)
    {
        services.AddSingleton<IProductMapper, ProductMapper>();
        services.AddSingleton<IOrderMapper, OrderMapper>();
    }
    
    private static void AddClientMappers(IServiceCollection services)
    {
        services.AddSingleton<IBalanceServiceMapper, BalanceServiceMapper>();
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IOrderRepository).GetTypeInfo().Assembly));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddDbContext<BpnDbContext>((_, optionsBuilder) => optionsBuilder.UseInMemoryDatabase("OrderDb"))
            .AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IOrderRepository, OrderRepository>();
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CreateOrderInputValidator).Assembly);
        services.AddFluentValidationAutoValidation();
    }
    
    private static void RegisterRedlock(this IServiceCollection services, ConfigurationManager configuration)
    {
        var servicesConfiguration = configuration.GetSection("Redis").Get<RedisSettings>();

        var redisEndpoints = new List<RedLockEndPoint>
        {
            new DnsEndPoint(servicesConfiguration.BaseAddress, servicesConfiguration.Port)
        };

        var redlockFactory = RedLockFactory.Create(redisEndpoints);

        services.AddSingleton<IDistributedLockFactory>(redlockFactory);
        services.AddSingleton<IRedisServiceClient>(new RedisServiceClient(redlockFactory, servicesConfiguration));
    }
}