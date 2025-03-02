using BuildBlocks.Domain.ValueObjects;
using BuildInBlocks.Messaging.Dtos;
namespace Cart.WebApi.Cart.CartCheckout;

public record CartCheckoutRequest
{
    public AddressDto ShippingAddress { get; init; } = default!;
    public AddressDto BillingAddress { get; init; } = default!;
}

public class CartCheckoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Cart/checkout", async (CartCheckoutRequest req, ISender sender, HttpContext httpContext) =>
        {
            var nameIdentifier = httpContext.Items["NameIdentifier"] as string;

            var command = new CartCheckoutCommand
            {
                ShippingAddress = req.ShippingAddress,
                BillingAddress = req.BillingAddress,
                CustomerId = Guid.Parse(nameIdentifier!)
            };

            await sender.Send(command);

            return Results.NoContent();

        }).WithName("CheckoutCart")
        .Produces(StatusCodes.Status204NoContent)
        .RequireAuthorization()
        .AddEndpointFilter<ExtractIdentifierFromTokenEndPointFilter>()
        .WithSummary("Checkout Cart")
        .WithDescription("Checkout Cart");
    }
}
