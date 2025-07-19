using BPN.ECommerce.Api.EndpointHandlers;

namespace BPN.ECommerce.Api.EndpointMappings;

public static class OrderEndpointMappings
{
    public static void RegisterOrderEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var orderEndpoints = endpointRouteBuilder.MapGroup("/orders");

        orderEndpoints.MapPost("", OrderHandlers.CreateOrder)
            .WithName("CreateOrder")
            .WithOpenApi();
    }
}