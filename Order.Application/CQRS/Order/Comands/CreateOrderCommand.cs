using Order.Application.CQRS.Abstractions;

namespace Order.Application.CQRS.Order.Comands
{
    public record CreateOrderCommand : ICommand<CreateOrderCommand>
    {

    }
}
