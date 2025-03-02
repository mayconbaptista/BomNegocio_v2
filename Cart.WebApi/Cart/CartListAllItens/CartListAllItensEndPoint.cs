
using Cart.WebApi.Dtos;

namespace Cart.WebApi.Cart.CartListAllItens;

public class CartListAllItensEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Cart/itens", async (ISender sender, HttpContext httpContext) =>
        {
            var nameIdentifier = httpContext.Items["NameIdentifier"] as string;

            var query = new CartListAllItensQuery(Guid.Parse(nameIdentifier!));

            var result = await sender.Send(query);

            var response = result.Adapt<CartItemDto>();

            return Results.Ok(result);

        }).WithName("ListAllItens")
        .Produces<List<CartItemDto>>(StatusCodes.Status200OK)
        .RequireAuthorization()
        .AddEndpointFilter<ExtractIdentifierFromTokenEndPointFilter>()
        .WithSummary("List All Itens")
        .WithDescription("List All Itens");
    }
}
