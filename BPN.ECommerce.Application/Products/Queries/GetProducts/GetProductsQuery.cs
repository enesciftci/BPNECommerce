using BPN.ECommerce.Application.Common;
using BPN.ECommerce.Application.Products.Exceptions;
using BPN.ECommerce.Application.Products.Mapper;
using BPN.ECommerce.Application.Products.Models;
using BPN.ECommerce.Application.Services.Balance;
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
    private readonly IBalanceServiceClient _balanceServiceClient;
    private readonly IProductMapper _productMapper;

    public GetProductsQueryHandler(IBalanceServiceClient balanceServiceClient, IProductMapper productMapper)
    {
        _balanceServiceClient = balanceServiceClient;
        _productMapper = productMapper;
    }

    public async Task<GetProductsQueryResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _balanceServiceClient.GetProducts();

        if(!products.Data.Any())
            throw new ProductNotFoundException("No products found");
        
        return _productMapper.MapToGetProductsQueryResult(products);
    }
}

public record GetProductsQueryResult : BaseResult
{
    public List<ProductModel> Data { get; set; }
}