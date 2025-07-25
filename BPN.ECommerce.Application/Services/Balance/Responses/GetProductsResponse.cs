namespace BPN.ECommerce.Application.Services.Balance.Responses;

public class GetProductsResponse
{
    public List<ProductDto> Data { get; set; }
    public bool Success { get; set; }
}

public class ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string Category { get; set; }
    public int Stock { get; set; }
}