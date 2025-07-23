using BPN.ECommerce.Application.Orders.Commands.CompleteOrder;
using BPN.ECommerce.Application.Orders.Commands.CreateOrder;
using BPN.ECommerce.Application.Orders.Inputs;
using BPN.ECommerce.Application.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BPN.ECommerce.Api.EndpointHandlers;

public class OrderHandlers
{
    public static async Task<Results<ProblemHttpResult, Created>> CreateOrder(
        [FromBody] CreateOrderInput input,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(CreateOrderCommand.Create(input), cancellationToken);

        return TypedResults.Created();
    }
    
    public static async Task<Results<ProblemHttpResult, Ok>> CompleteOrder(
        [FromBody] CompleteOrderInput input,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(CompleteOrderCommand.Create(input), cancellationToken);

        return TypedResults.Ok();
    }
    
    public static async Task<Results<ProblemHttpResult, Ok<GetProductsQueryResult>>> GetProducts(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var output = await mediator.Send(GetProductsQuery.Create(), cancellationToken);

        return TypedResults.Ok(output);
    }
}