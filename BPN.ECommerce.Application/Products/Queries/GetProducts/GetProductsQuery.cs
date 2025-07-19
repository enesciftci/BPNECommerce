using BPN.ECommerce.Application.Common;
using BPN.ECommerce.Application.Products.Models;
using MediatR;

namespace BPN.ECommerce.Application.Products.Queries.GetProducts;

public sealed class GetProductsQuery : IRequest<GetProductsQueryResult>
{
    public static GetProductsQuery Create()
    {
        return new GetProductsQuery();
    }
}

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsQueryResult>
{
    public async Task<GetProductsQueryResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {


        return new GetProductsQueryResult();
    }
}

public record GetProductsQueryResult : BaseResult
{
    public List<ProductModel> Data { get; set; }
}