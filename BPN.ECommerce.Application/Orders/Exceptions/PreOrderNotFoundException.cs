using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Application.Orders.Exceptions;

public class PreOrderNotFoundException : CustomException
{
    public PreOrderNotFoundException(string description) : base(description)
    {
    }
}