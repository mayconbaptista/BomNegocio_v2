
using MediatR;

namespace Catalog.Api.ProductEndPoints.ProductsFilter
{
    public class ProductsFilterEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/filter/", async (ISender sender) =>
            {
                var response = await sender.Send(new ProductsFilterCommand());

                return Results.Ok(response);

            }).WithName("FilterProducts")
           .Produces(StatusCodes.Status200OK)
           .WithSummary("Filter products")
           .WithDescription("filter product by ...");
        }
    }
}
