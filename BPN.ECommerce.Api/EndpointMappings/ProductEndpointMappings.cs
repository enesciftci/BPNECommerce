using BPN.ECommerce.Api.EndpointHandlers;

namespace BPN.ECommerce.Api.EndpointMappings;

public static class ProductEndpointMappings
{
    public static void RegisterProductEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var productEndpoints = endpointRouteBuilder.MapGroup("/products");

        productEndpoints.MapGet("", ProductHandlers.GetProducts)
            .WithName("GetProducts")
            .WithOpenApi();
    }
}