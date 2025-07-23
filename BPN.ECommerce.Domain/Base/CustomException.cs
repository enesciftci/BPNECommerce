namespace BPN.ECommerce.Domain.Base;

public abstract class CustomException : Exception
{
    public string Description { get; set; }

    protected CustomException(string description) : base(description)
    {
        Description = description;
    }
}