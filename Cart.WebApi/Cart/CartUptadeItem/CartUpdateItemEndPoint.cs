
using BuildInBlocks.Messaging.Dtos;
using Cart.WebApi.Cart.CarAddItem;
using Microsoft.AspNetCore.Mvc;

namespace Cart.WebApi.Cart.CartUptadeItem;

public record CartUpdateItemRequest (Guid ProductId, int Quantity);

public record CartUpdateItemEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/Cart/update", async (ISender sender, [FromBody] CartUpdateItemRequest req, HttpContext context) =>
        {
            var nameIdentifier = context.Items["NameIdentifier"] as string;

            var command = new CartUpdateItemCommand(
                CartId: Guid.Parse(nameIdentifier!),
                ProductId: req.ProductId,
                Quantity: req.Quantity);

            var result = await sender.Send(command);

            return Results.Ok(result.Adapt<CartItemDto>());

        }).WithName("CartUpdate")
        .Produces<CartAddItemResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization()
        .AddEndpointFilter<ExtractIdentifierFromTokenEndPointFilter>()
        .WithSummary("Cart Update")
        .WithDescription("Cart update Item");
    }
}
