using Catalog.Api.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.ProductEndPoints.GetProductsByCategory;

public record ProductFilterRequest(Guid? categoryId);

public class GetProductsByCategoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/product/category/{categoryId:guid}", async ([FromRoute] Guid categoryId, ISender sender) =>
        {
            var query = new GetProductsByCategoryQuery(categoryId);

            var result = await sender.Send(query);

            var response = result.Adapt<ProductDto>();

            return Results.Ok(result);

        }).WithName("GetProductsByCategory");
    }
}
