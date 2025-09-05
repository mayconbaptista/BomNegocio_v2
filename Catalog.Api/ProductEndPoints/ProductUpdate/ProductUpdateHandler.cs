using BuildBlocks.Domain.Abstractions.CQRS;
using BuildBlocks.Domain.Exceptions;
using Catalog.Api.Data.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.Api.ProductEndPoints.ProductUpdate;

public record ProductUpdateCommand : ICommand
{
    public Guid Id { get; init; }
    public uint? Quantity { get; init; }

    public decimal? UnitPrice { get; init; }

    public string? ProductName { get; init; }

    public string? ProductDescription { get; init; }
};

public class ProductUpdateHandler(IUnitOfWork unitOfWork, ILogger<ProductUpdateHandler> logger) 
    : ICommandHandler<ProductUpdateCommand>
{
    public async Task<Unit> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.IProductRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Produto não encontrado");

        if(!request.ProductName.IsNullOrEmpty()) product.Name = request.ProductName!;
        if(request.UnitPrice.HasValue) product.Price = request.UnitPrice.Value;
        if(!request.ProductDescription.IsNullOrEmpty()) product.Description = request.ProductDescription!;
        if(request.Quantity.HasValue) product.Quantity = request.Quantity.Value!;

        return Unit.Value;
    }
}
