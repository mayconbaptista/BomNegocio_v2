
using Microsoft.AspNetCore.Mvc;

namespace Cart.WebApi.Cart.CartRemoveItem;

public record CartRemoveItemRequest
{
    [Required(ErrorMessage = "O identificador do produto é obrigatório")]
    public Guid ProductId { get; init; }
}

public class CartRemoveItemEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/Cart/remove/{productId:guid}", async (Guid productId, ISender sender, HttpContext httpContext) =>
        {
            var nameIdentifier = httpContext.Items["NameIdentifier"] as string;

            var command = new CartRemoveItemCommand(
                    customerId: Guid.Parse(nameIdentifier!),
                    productId: productId);

            await sender.Send(command);

            return Results.NoContent();

        }).WithName("RemoveProduct")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .AddEndpointFilter<ExtractIdentifierFromTokenEndPointFilter>()
        .WithSummary("Remove Product")
        .WithDescription("Remove Product");
    }
}

