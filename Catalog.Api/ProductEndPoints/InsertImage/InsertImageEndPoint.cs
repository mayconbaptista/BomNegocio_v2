using Carter;
using MediatR;

namespace Catalog.Api.ProductEndPoints.InsertOrUpdateImage;

public record InsertImageResponse(string filePath);

public sealed class InsertImageEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/product/{productId:guid}/image", async (IFormFile file, Guid productId, ISender sender) =>
        {
            using (var stream = new MemoryStream())
            {
                var command = new InsertImageCommand(productId: productId, file.FileName, stream);

                var path = await sender.Send(command);

                return Results.Created(string.Empty, new InsertImageResponse(path));
            }
        }).WithName("InsertImage")
       .Produces<InsertImageResponse>(StatusCodes.Status201Created)
       .WithSummary("Insert Image")
       .WithDescription("Insert product image")
       .Accepts<IFormFile>("multipart/form-data")
       .DisableAntiforgery();
    }
}
