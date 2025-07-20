using BPN.ECommerce.Application.Services.Balance.Responses;
using BPN.ECommerce.Infrastructure.Services.Balance.Responses;

namespace BPN.ECommerce.Infrastructure.Services.Balance.Mapper;

public interface IBalanceServiceMapper
{
    GetProductsResponse MapToGetProductsResponse(GetProductsServiceResponse response);
}
public class BalanceServiceMapper : IBalanceServiceMapper
{
    public GetProductsResponse MapToGetProductsResponse(GetProductsServiceResponse response)
    {
        if (response?.Data == null)
        {
            return new GetProductsResponse();
        }

        return new GetProductsResponse()
        {
            Success = response.Success,
            Data = response.Data.Select(p => new ProductDto()
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