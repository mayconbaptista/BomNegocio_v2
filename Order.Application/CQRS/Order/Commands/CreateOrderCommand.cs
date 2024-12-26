//using MediatR;
using BuildBlocks.Domain.ValueObjects;
using Mapster;
using Microsoft.Extensions.Logging;
using Order.Domain.Interfaces;
using Order.Domain.Entities;
using Order.Domain.ValueObjects;

namespace Order.Application.CQRS.Order.Commands;

public sealed record CreateOrderCommand : ICommand<Guid>
{
    public AddressDto ShippingAdress { get; init; }
    public AddressDto BillingAdress { get; init; }
    public CustomerDto Customer { get; init; }
    public List<OrderItemDto> OrderItems { get; init; }
}

public sealed class CreateOrderCommandHandler
    (IUnitOfWork unitOfWork, ILogger<CreateOrderCommand> logger) 
    : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly ILogger<CreateOrderCommand> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entt = await this._unitOfWork.OrderRepository.AddAsync(this.CreateNewOrder(request));

        await this._unitOfWork.CommitAsync(cancellationToken);

        return entt.Id;
    }

    public OrderEntity CreateNewOrder(CreateOrderCommand request)
    {
        var shippingAddress = request.ShippingAdress.Adapt<Address>();

        var billingAddress = request.BillingAdress.Adapt<Address>();

        var items = request.OrderItems.Adapt<List<OrderItemEntity>>();

        return OrderEntity.Create(request.Customer.Id, shippingAddress, billingAddress, items);
    }
}