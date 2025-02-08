using Catalog.Api.Data.Interfaces;
using Catalog.Api.Dtos;
using Mapster;
using MediatR;

namespace Catalog.Api.Product.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDetailsDto>;

public class GetProductByIdHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProductByIdQuery, ProductDetailsDto>
{
    public async Task<ProductDetailsDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.IProductRepository.GetByIdAsync(request.Id);

        return product.Adapt<ProductDetailsDto>();
    }
}
