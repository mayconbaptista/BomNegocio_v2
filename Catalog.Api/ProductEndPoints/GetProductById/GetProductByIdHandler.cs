using BuildBlocks.Domain.Exceptions;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Dtos;
using Mapster;
using MediatR;

namespace Catalog.Api.ProductEndPoints.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDetailsDto>;

public class GetProductByIdHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProductByIdQuery, ProductDetailsDto>
{
    public async Task<ProductDetailsDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.IProductRepository.GetByIdWithDetais(request.Id)
            ?? throw new BadRequestException("O produto não foi encontrado.");

        return product.Adapt<ProductDetailsDto>();
    }
}
