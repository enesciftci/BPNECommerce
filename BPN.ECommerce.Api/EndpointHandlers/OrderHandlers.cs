using BPN.ECommerce.Application.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BPN.ECommerce.Api.EndpointHandlers;

public class OrderHandlers
{
    public static async Task<Results<ProblemHttpResult, Created>> CreateOrder(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var output = await mediator.Send(GetProductsQuery.Create(), cancellationToken);

        return TypedResults.Created();
    }
}