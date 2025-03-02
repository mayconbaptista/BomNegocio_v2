using BuildBlocks.WebApi.Filters;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Product.CreateProduct;

public record CreateProductRequest
{
    [Required(ErrorMessage = "O nome do produto é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome do produto deve ter no máximo {1} caracteres")]
    [MinLength(2, ErrorMessage = "O nome do produto deve ter no mínimo {1} caracteres")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "O código do produto é obrigatório")]
    [MaxLength(16, ErrorMessage = "O código do produto deve ter no máximo {1} caracteres")]
    [MinLength(4, ErrorMessage = "O código do produto deve ter no mínimo {1} caracteres")]
    [param: RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "O código do produto deve conter apenas letras e números")]
    public string SkuCode { get; set; } = default!;

    [MaxLength(500, ErrorMessage = "A descrição do produto deve ter no máximo {1} caracteres")]
    public string Description { get; set; } = default!;

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço do produto deve ser maior que zero")]
    public decimal Price { get; set; } = default!;

    [Range(1, uint.MaxValue, ErrorMessage = "A quantidade do produto deve ser maior que zero")]
    public uint Quantity { get; set; } = default!;

    [Required(ErrorMessage = "O identificador da categoria é obrigatório")]
    public Guid CategoryId { get; set; } = default!;
}

public record CreateProductResponse(Guid ProductId);

public class CreateProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/product/create", async ([FromBody] CreateProductRequest req, ISender sender, HttpContext context) =>
        {
            //var nameIdentifier = context.Items["NameIdentifier"] as string;

            var command = new CreateProductCommand
            {
                Name = req.Name,
                SkuCode = req.SkuCode,
                Description = req.Description,
                Price = req.Price,
                Quantity = req.Quantity,
                CategoryId = req.CategoryId
            };
  
            var id = await sender.Send(command);

            return Results.Created("/product/create", new CreateProductResponse(id));

        }).WithName("CreateProduct")
       .Produces(StatusCodes.Status201Created)
       .WithSummary("Create product")
       .WithDescription("Create product in catalog");
    }
}