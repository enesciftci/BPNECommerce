using BPN.ECommerce.Application.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BPN.ECommerce.Api.EndpointHandlers;

public static class ProductHandlers
{
    public static async Task<Results<ProblemHttpResult, Ok<GetProductsQueryResult>>> GetProducts(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var output = await mediator.Send(GetProductsQuery.Create(), cancellationToken);

        return TypedResults.Ok(output);
    }

}