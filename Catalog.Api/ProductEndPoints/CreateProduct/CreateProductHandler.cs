using BuildBlocks.Domain.Abstractions.CQRS;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Mapster;
using MediatR;

namespace Catalog.Api.ProductEndPoints.CreateProduct;

public record CreateProductCommand : ICommand<Guid>
{
    public string Name { get; set; } = default!;
    public string SkuCode { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public uint Quantity { get; set; } = default!;
    public Guid CategoryId { get; set; } = default!;
}

public class CreateProductHandler(IUnitOfWork unitOfWork, ILogger<CreateProductHandler> logger)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductEntity
        {
            Name = request.Name,
            SkuCode = request.SkuCode,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            CategoryId = request.CategoryId
        };

        await unitOfWork.IProductRepository.AddAsync(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
