namespace BPN.ECommerce.Application.Common;

public abstract record BaseResult
{
    public bool Success { get; init; }
}