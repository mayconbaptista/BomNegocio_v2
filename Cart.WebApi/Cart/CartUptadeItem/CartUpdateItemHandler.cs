
using BuildBlocks.Domain.Exceptions;

namespace Cart.WebApi.Cart.CartUptadeItem;

public record CartUpdateItemCommand(Guid CartId, Guid ProductId, int Quantity) : ICommand<CartItemEntity>;

public class CartUpdateItemHandler(IUnitOfWork unitOfWork) : ICommandHandler<CartUpdateItemCommand, CartItemEntity>
{
    public async Task<CartItemEntity> Handle(CartUpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await unitOfWork.CartRepository.GetItemAsync(request.CartId, request.ProductId);

        if (item is null)
        {
            throw new BadRequestException("Produto não encontrato no carrinho");
        }

        if((item.Quantity + request.Quantity) <= 0)
        {
            unitOfWork.CartRepository.RemoveItem(item!);
        }
        else
        {
            item.Quantity = ((uint)(item.Quantity + request.Quantity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return item;
    }
}
