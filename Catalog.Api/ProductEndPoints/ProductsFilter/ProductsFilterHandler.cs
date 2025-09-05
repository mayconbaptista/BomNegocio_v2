using BuildBlocks.Domain.Abstractions.CQRS;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Dtos;
using Mapster;
using MediatR;

namespace Catalog.Api.ProductEndPoints.ProductsFilter;

public record ProductsFilterCommand : ICommand<ICollection<ProductDto>>
{
    public string? Category { get; set; }
}

public class ProductsFilterHandler(IUnitOfWork unitOfWork) 
    : ICommandHandler<ProductsFilterCommand, ICollection<ProductDto>>
{
    public async Task<ICollection<ProductDto>> Handle(ProductsFilterCommand request, CancellationToken cancellationToken)
    {
        var products = await unitOfWork.IProductRepository.GetAllAsync();

        return products.Adapt<ICollection<ProductDto>>();
    }
}
