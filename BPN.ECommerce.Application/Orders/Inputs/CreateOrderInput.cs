namespace BPN.ECommerce.Application.Orders.Inputs;

public class CreateOrderInput
{
    public List<OrderLine> Items { get; set; }
}

public record OrderLine(string ProductId, int Quantity);