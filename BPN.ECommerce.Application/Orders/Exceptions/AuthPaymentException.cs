using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Application.Orders.Exceptions;

public class AuthPaymentException : CustomException
{
    public AuthPaymentException(string description) : base(description)
    {
    }
}