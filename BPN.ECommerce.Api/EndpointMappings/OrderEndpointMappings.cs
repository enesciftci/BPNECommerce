using BPN.ECommerce.Api.EndpointHandlers;
using BPN.ECommerce.Api.Filters;
using BPN.ECommerce.Application.Orders.Inputs;

namespace BPN.ECommerce.Api.EndpointMappings;

public static class OrderEndpointMappings
{
    public static void RegisterOrderEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var orderEndpoints = endpointRouteBuilder.MapGroup("/orders");

        orderEndpoints.MapPost("create", OrderHandlers.CreateOrder)
            .WithName("CreateOrder")
            .WithOpenApi()
            .AddEndpointFilter<ValidationFilter<CreateOrderInput>>();

        orderEndpoints.MapPost("complete", OrderHandlers.CompleteOrder)
            .WithName("CompleteOrder")
            .WithOpenApi()
            .AddEndpointFilter<ValidationFilter<CompleteOrderInput>>();
    }
}