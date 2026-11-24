
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.ProductEndPoints.DeleteImage;

public record DeleteImageRequest(uint ImageId, Guid ProductId);

public class DeleteImageEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("product/image", async ([FromBody] DeleteImageRequest request, ISender sender) =>
        {
            await sender.Send(new DeleteImageCommand(request.ProductId, request.ImageId));

            return Results.NoContent();

        }).WithName("DeleteProductImage")
       .Produces(StatusCodes.Status204NoContent)
       .WithSummary("Delete Product Image")
       .WithDescription("Delete product image");
    }
}
