using Catalog.Api.Data.Interfaces;
using Catalog.Api.Dtos;
using Mapster;
using MediatR;

namespace Catalog.Api.ProductEndPoints.GetProductsByCategory;

public record GetProductsByCategoryQuery(Guid? CategoryId) : IRequest<ICollection<ProductDto>>;

public class GetProductsByCategoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductsByCategoryQuery, ICollection<ProductDto>>
{

    public async Task<ICollection<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await unitOfWork.IProductRepository.GetByCategoryAsync(request.CategoryId);

        return products.Adapt<ICollection<ProductDto>>();
    }
}

