using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Application.Products.Exceptions;

public class ProductNotFoundException : CustomException
{
    public ProductNotFoundException(string description)
    {
        Description = description;
    }
}