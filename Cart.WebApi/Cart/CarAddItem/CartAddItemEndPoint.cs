using BuildBlocks.WebApi.Filters;
using Cart.WebApi.Cart.CartCheckout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cart.WebApi.Cart.CarAddItem;

public record CartAddItemRequest
{
    [Required(ErrorMessage = "O identificador do produto é obrigatório")]
    public Guid ProductId { get; init; }

    [Range(1, uint.MaxValue, ErrorMessage = "A quantidade do produto deve ser maior que zero")]
    public uint Quantity { get; init; }
}

public record CartAddItemResponse(uint Quantity);

public record CartAddItemEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Cart/add", async ([FromBody] CartAddItemRequest req, ISender sender, HttpContext context) =>
        {
            var nameIdentifier = context.Items["NameIdentifier"] as string;

            var command = new CartAddItemCommand(
                CustomerId: Guid.Parse(nameIdentifier!), 
                ProductId: req.ProductId, 
                Quantity: req.Quantity);

            var result = await sender.Send(command);

            return Results.Ok(new CartAddItemResponse(result));

        }).WithName("CartAdd")
        .Produces<CartAddItemResponse>(StatusCodes.Status200OK)
        .RequireAuthorization()
        .AddEndpointFilter<ExtractIdentifierFromTokenEndPointFilter>()
        .WithSummary("Cart Add")
        .WithDescription("Cart Add Item");
    }
}
