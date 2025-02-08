using BuildBlocks.Domain.Abstractions.CQRS;
using BuildBlocks.Domain.Exceptions;
namespace Cart.WebApi.Cart.CartRemoveItem;


public record CartRemoveItemCommand(Guid customerId, Guid productId) : ICommand<CartRemoveItemResponse>;

public record CartRemoveItemResponse(bool isSucess = true);

public class CartRemoveItemHandler(IUnitOfWork unitOfWork) : ICommandHandler<CartRemoveItemCommand, CartRemoveItemResponse>
{
    public async Task<CartRemoveItemResponse> Handle(CartRemoveItemCommand request, CancellationToken cancellationToken)
    {
        var entt = await unitOfWork.CartRepository.GetItemAsync(request.customerId, request.productId);
        //?? throw new NotFoundException($"O produto {request.productId} não foi encontrado!.");

        if (entt is null) throw new NotFoundException("Produto não encontrado");

        unitOfWork.CartRepository.RemoveItem(entt);
        await unitOfWork.SaveChangesAsync();

        return new CartRemoveItemResponse();
    }
}
