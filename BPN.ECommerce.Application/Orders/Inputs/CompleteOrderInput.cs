using FluentValidation;

namespace BPN.ECommerce.Application.Orders.Inputs;

public class CompleteOrderInput
{
    public string OrderId { get; set; }
}

public class CompleteOrderInputValidator : AbstractValidator<CompleteOrderInput>
{
    public CompleteOrderInputValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().NotNull().WithMessage("OrderId is required.");
    }
}