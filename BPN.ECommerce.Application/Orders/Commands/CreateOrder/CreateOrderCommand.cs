using BPN.ECommerce.Application.Products.Queries.GetProducts;
using MediatR;

namespace BPN.ECommerce.Application.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommand : IRequest
{

}

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}