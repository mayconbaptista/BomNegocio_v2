using BuildBlocks.Domain.Abstractions.CQRS;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Catalog.Api.ProductEndPoints.ProductsFilter;

public record ProductsFilterCommand : ICommand<ICollection<ProductDto>>
{
    public string? Category { get; set; }
    public List<Guid> Ids { get; set; } = new();
}

public class ProductsFilterHandler(IUnitOfWork unitOfWork) 
    : ICommandHandler<ProductsFilterCommand, ICollection<ProductDto>>
{
    public async Task<ICollection<ProductDto>> Handle(ProductsFilterCommand request, CancellationToken cancellationToken)
    {
        List<Expression<Func<ProductEntity, bool>>> filters = new();

        if (!string.IsNullOrEmpty(request.Category))
        {
            filters.Add(x => x.Category.Name.ToLower() == request.Category.ToLower());
        }

        if(request.Ids is not null && request.Ids.Any())
        {
            filters.Add(x => request.Ids.Contains(x.Id));
        }

        var products = await unitOfWork
            .IProductRepository
            .Filter(filters);

        return products.Adapt<ICollection<ProductDto>>();
    }
}
