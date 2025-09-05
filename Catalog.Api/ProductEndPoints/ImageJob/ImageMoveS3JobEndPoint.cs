using MediatR;

namespace Catalog.Api.ProductEndPoints.ImageJob;

public class ImageMoveS3JobEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/product/imagejob", async (ISender sender) =>
        {
            await sender.Send(new ImageMoveS3JobCommand(string.Empty));

            return Results.NoContent();
        });
    }
}
