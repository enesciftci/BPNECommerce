using FluentValidation;

namespace BPN.ECommerce.Application.Orders.Inputs;

public class CreateOrderInput
{
    public string OrderId { get; set; }
    public List<OrderLine> Items { get; set; }
}

public record OrderLine(string ProductId, int Quantity);

public class CreateOrderInputValidator : AbstractValidator<CreateOrderInput>
{
    public CreateOrderInputValidator()
    {
        RuleFor(p => p.Items)
            .NotNull()
            .WithMessage("Items list cannot be null.")
            .Must(items => items.Count > 0)
            .WithMessage("Item count must be greater than 0");

        RuleFor(p=>p.OrderId).NotNull().NotEmpty().WithMessage("OrderId cannot be empty.");
        RuleForEach(p => p.Items).SetValidator(new OrderLineValidator());
    }
}

public class OrderLineValidator : AbstractValidator<OrderLine>
{
    public OrderLineValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be empty.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}