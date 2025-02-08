

using BuildBlocks.Domain.Abstractions.CQRS;

namespace Cart.WebApi.Cart.CartListAllItens;

public record CartListAllItensQuery(Guid customerId) : IQuery<ICollection<CartItemEntity>>;

public class CartListAllItensHandler(IUnitOfWork unitOfWork) : IQueryHandler<CartListAllItensQuery, ICollection<CartItemEntity>>
{
    public async Task<ICollection<CartItemEntity>> Handle(CartListAllItensQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.CartRepository.GetAllAsync(request.customerId);
    }
}
