using BPN.ECommerce.Application.Products.Models;
using BPN.ECommerce.Application.Products.Queries.GetProducts;
using BPN.ECommerce.Application.Services.Balance.Responses;

namespace BPN.ECommerce.Application.Products.Mapper;

public interface IProductMapper
{
    GetProductsQueryResult MapToGetProductsQueryResult(GetProductsResponse response);
}
public class ProductMapper : IProductMapper
{
    public GetProductsQueryResult MapToGetProductsQueryResult(GetProductsResponse response)
    {
        return new GetProductsQueryResult()
        {
            Success = response.Success,
            Data = response.Data.Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category,
                Currency = p.Currency,
                Stock = p.Stock
            }).ToList()
        };
    }
}