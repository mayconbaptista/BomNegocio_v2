using BuildBlocks.Domain.ValueObjects;
using Order.Domain.Interfaces;
using Order.Domain.Entities;
using BuildInBlocks.Messaging.Dtos;
using BuildBlocks.Domain.Abstractions.CQRS;
using Order.Domain.ValueObjects;
using Order.Domain.Enums;
using Order.Application.Common.Interfaces;

namespace Order.Application.CQRS.Order.Commands;

public sealed record CreateOrderCommand : ICommand<Guid>
{
    public AddressDto ShippingAdress { get; init; }
    public AddressDto BillingAdress { get; init; }
    public CustomerDto Customer { get; init; }
    public DeliveryDto Delivery { get; init; }
    public List<OrderItemDto> OrderItems { get; init; } = new();
}

public sealed class CreateOrderCommandHandler
    (IUnitOfWork unitOfWork, IPaymentProcessor paymentProcessor)
    : ICommandHandler<CreateOrderCommand, Guid>
{

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var price = request.OrderItems.Sum(x => x.Quantity * x.UnitPrice);

        var paymentEtt = await paymentProcessor.ProcessPixPayment(request.Customer.Documento, request.Customer.Name, price);

        var entt = this.CreateNewOrderPix(request, paymentEtt);

        await unitOfWork.OrderRepository.AddAsync(entt);

        foreach (var item in request.OrderItems)
        {
            entt.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return entt.Id;
    }

    public OrderEntity CreateNewOrderPix(CreateOrderCommand request, PaymentEntity payment)
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
            request.Customer.Documento,
            shippingAddress,
            billingAddress,
            payment,
            Delivery.Create(
                (DeliveryType)request.Delivery.Type,
                request.Delivery.EstimatedDeliveryDate,
                request.Delivery.Price)
            );

        return entt;
    }
}