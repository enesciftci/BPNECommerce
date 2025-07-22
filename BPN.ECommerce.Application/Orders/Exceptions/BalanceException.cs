using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Application.Orders.Exceptions;

public class BalanceException : CustomException
{
    public BalanceException(string description) : base(description)
    {
    }
}