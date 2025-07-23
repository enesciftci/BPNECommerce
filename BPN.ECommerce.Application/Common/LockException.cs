using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Application.Common;

public class LockException : CustomException
{
    public LockException(string description) : base(description)
    {
    }
}