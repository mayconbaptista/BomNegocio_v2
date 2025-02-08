
using BuildBlocks.Domain.Abstractions.CQRS;

namespace Cart.WebApi.Cart.CarAddItem;

public record CartAddItemCommand(Guid CustomerId, Guid ProductId, uint Quantity) : ICommand<uint>;

public class CartAddItemHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CartAddItemCommand, uint>
{
    public async Task<uint> Handle(CartAddItemCommand request, CancellationToken cancellationToken)
    {

        var cart = await unitOfWork.CartRepository.GetCartAsync(request.CustomerId);
        
        var newProduct = new CartItemEntity
        {
            CartId = request.CustomerId,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        if(cart is null)
        {
            var newCart = new CartEntity
            {
                Id = request.CustomerId
            };
            await unitOfWork.CartRepository.CreateCart(newCart, cancellationToken);
            await unitOfWork.CartRepository.AddItemAsync(newProduct, cancellationToken);
        }
        else 
        {
            var product = cart.Items.FirstOrDefault(x => x.ProductId == newProduct.ProductId);

            if(product is null)
            {
                await unitOfWork.CartRepository.AddItemAsync(newProduct, cancellationToken);
            }
            else
            {
                product.Quantity += newProduct.Quantity;
                newProduct.Quantity = product.Quantity;
            }
        }

        await unitOfWork.SaveChangesAsync();

        return newProduct.Quantity;
    }
}

