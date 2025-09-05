
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.ProductEndPoints.ProductUpdate;

internal record ProductUpdateRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "O novo valor do produto deve ser maior que zero.")]
    public decimal? UnitPrice { get; set; }

    [StringLength(10, ErrorMessage = "O produto não pode ter mais que 50 caracteres.")]
    public string? ProductName { get; set; }

    [Range(10, 500, ErrorMessage = "A descrição deve ter entre {0} e {1} caracteres.")]
    public string? ProductDescription { get; set; }
};

public class ProductUpdateEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("product/update", async([FromBody] ProductUpdateRequest request, ISender sender) =>
        {
            await sender.Send(new ProductUpdateCommand
            {
                Id = request.ProductId,
                ProductName = request.ProductName,
                ProductDescription = request.ProductDescription,
                UnitPrice = request.UnitPrice
            });

            return Results.NoContent();

        }).WithName("UpdateProduct")
       .Produces(StatusCodes.Status204NoContent)
       .WithSummary("Update product")
       .WithDescription("Update product");
    }
}
