namespace BPN.ECommerce.Application.Products.Models;

public class ProductModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; }
    public string Category { get; init; }
    public int Stock { get; init; }
}