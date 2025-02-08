using BuildBlocks.Domain.Abstractions.CQRS;
using BuildInBlocks.Messaging.Dtos;
using BuildInBlocks.Messaging.Events;
using MassTransit;
using Microsoft.IdentityModel.Tokens;

namespace Cart.WebApi.Cart.CartCheckout;

public record CartCheckoutCommand : ICommand<CartCheckoutResult>
{
    public Guid CustomerId { get; init; }
    public AddressDto ShippingAddress { get; init; }
    public AddressDto BillingAddress { get; init; }
}

public record CartCheckoutResult(bool IsSuccess, string? ErrorMessage = null);

public class CartCheckoutHandler(IUnitOfWork unitOfWork, IPublishEndpoint publish) 
    : ICommandHandler<CartCheckoutCommand, CartCheckoutResult>
{
    public async Task<CartCheckoutResult> Handle(CartCheckoutCommand request, CancellationToken cancellationToken)
    {
        var cart = await unitOfWork.CartRepository.GetCartAsync(request.CustomerId);

        if (cart is null || cart?.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty");
        }

        var cartCheckoutEvent = new CartCheckoutEvent
        {
            Customer = new CustomerDto(request.CustomerId, "User name", "maycon@ufu.com.br"),
            ShippingAddress = request.ShippingAddress,
            BillingAddress = request.BillingAddress,
            Itens = cart!.Items.Select(x => new CartItemDto(x.ProductId, x.Quantity,x.Quantity)).ToList()
        };

        await publish.Publish(cartCheckoutEvent, cancellationToken);

        unitOfWork.CartRepository.Clear(cart!.Items);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CartCheckoutResult(true);
    }
}
