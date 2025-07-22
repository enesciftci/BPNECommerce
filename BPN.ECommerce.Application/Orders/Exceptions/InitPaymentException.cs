using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Application.Orders.Exceptions;

public class InitPaymentException : CustomException
{
    public InitPaymentException(string description) : base(description)
    {
    }
}