using BuildBlocks.Domain.ValueObjects;
using Order.Domain.Interfaces;
using Order.Domain.Entities;
using BuildInBlocks.Messaging.Dtos;
using BuildBlocks.Domain.Abstractions.CQRS;
using Order.Domain.ValueObjects;
using Order.Domain.Enums;

namespace Order.Application.CQRS.Order.Commands;

public sealed record CreateOrderCommand : ICommand<Guid>
{
    public AddressDto ShippingAdress { get; init; }
    public AddressDto BillingAdress { get; init; }
    public CustomerDto Customer { get; init; }
    public PaymentDto Payment { get; init; }
    public DeliveryDto Delivery { get; init; }
    public List<OrderItemDto> OrderItems { get; init; } = new();
}

public sealed class CreateOrderCommandHandler
    (IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateOrderCommand, Guid>
{

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entt = this.CreateNewOrder(request);

        await unitOfWork.OrderRepository.AddAsync(entt);

        foreach (var item in request.OrderItems)
        {
            entt.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return entt.Id;
    }

    public OrderEntity CreateNewOrder(CreateOrderCommand request)
    {
        var shippingAddress = new Address(
            request.ShippingAdress.Name,
            request.ShippingAdress.Street,
            request.ShippingAdress.City,
            request.ShippingAdress.State,
            request.ShippingAdress.Country,
            request.ShippingAdress.ZipCode);

        var billingAddress = new Address(
            request.BillingAdress.Name,
            request.BillingAdress.Street,
            request.BillingAdress.City,
            request.BillingAdress.State,
            request.BillingAdress.Country,
            request.BillingAdress.ZipCode);

        OrderEntity entt = OrderEntity.Create(
            request.Customer.Id,
            shippingAddress,
            billingAddress,
            Payment.Create(
                request.Payment.Type,
                request.Payment.CardNumber,
                request.Payment.CardHolderName,
                request.Payment.CardExpirationDate,
                request.Payment.CardCvv,
                null),
            Delivery.Create(
                (DeliveryType) request.Delivery.Type,
                request.Delivery.EstimatedDeliveryDate,
                request.Delivery.Price)
            );

        return entt;
    }

    public OrderEntity CreateNewOrderPix(CreateOrderCommand request)
    {
        var shippingAddress = new Address(
            request.ShippingAdress.Name,
            request.ShippingAdress.Street,
            request.ShippingAdress.City,
            request.ShippingAdress.State,
            request.ShippingAdress.Country,
            request.ShippingAdress.ZipCode);

        var billingAddress = new Address(
            request.BillingAdress.Name,
            request.BillingAdress.Street,
            request.BillingAdress.City,
            request.BillingAdress.State,
            request.BillingAdress.Country,
            request.BillingAdress.ZipCode);

        OrderEntity entt = OrderEntity.Create(
            request.Customer.Id,
            shippingAddress,
            billingAddress,
            Payment.PIX(string.Empty),
            Delivery.Create(
                (DeliveryType)request.Delivery.Type,
                request.Delivery.EstimatedDeliveryDate,
                request.Delivery.Price)
            );

        return entt;
    }
}