using System.Reflection;
using BPN.ECommerce.Application.Orders;
using BPN.ECommerce.Application.Products.Mapper;
using BPN.ECommerce.Infrastructure.Services.Balance.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace BPN.ECommerce.Infrastructure.ServiceRegistrations;

public static class Registrations
{
    public static void RegisterServices(this IServiceCollection services)
    {
        AddMediatR(services);
        AddApiMappers(services);
        AddClientMappers(services);
    }
    private static void AddApiMappers(IServiceCollection services)
    {
        services.AddSingleton<IProductMapper, ProductMapper>();
    }
    
    private static void AddClientMappers(IServiceCollection services)
    {
        services.AddSingleton<IBalanceServiceMapper, BalanceServiceMapper>();
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IOrderRepository).GetTypeInfo().Assembly));
    }
}