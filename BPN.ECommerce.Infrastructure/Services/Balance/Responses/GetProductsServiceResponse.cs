namespace BPN.ECommerce.Infrastructure.Services.Balance.Responses;

public class GetProductsServiceResponse
{
    public List<ProductData> Data { get; set; }
    public bool Success { get; set; }
}

public class ProductData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string Category { get; set; }
    public int Stock { get; set; }
}