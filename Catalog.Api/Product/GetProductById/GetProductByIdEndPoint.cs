using Carter;
using MediatR;

namespace Catalog.Api.Product.GetProductById;

public record GetProductByIdResponse
{
    public string Name { get; set; } = default!;
    public string SkuCode { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public uint Quantity { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
}

public class GetProductByIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/product/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetProductByIdQuery(id);

            var result = await sender.Send(query);

            return result is null ? Results.NotFound() : Results.Ok(result);

        }).WithName("GetProductById");
    }
}
