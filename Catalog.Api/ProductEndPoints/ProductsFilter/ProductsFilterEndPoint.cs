
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.ProductEndPoints.ProductsFilter;


public class ProductsFilterEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/product/filter/", async (ISender sender, HttpRequest req) =>
        {
            var command = new ProductsFilterCommand
            {
                Category = req.Query["category"],
                Ids = req.Query["ids"].Select(Guid.Parse).ToList()
            };

            var response = await sender.Send(command);

            return Results.Ok(response);

        }).WithName("FilterProducts")
        .AllowAnonymous()
       .Produces(StatusCodes.Status200OK)
       .WithSummary("Filter products")
       .WithDescription("filter product by ...");
    }
}
