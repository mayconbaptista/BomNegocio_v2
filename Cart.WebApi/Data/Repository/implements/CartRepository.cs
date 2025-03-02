using Cart.WebApi.Data.Repository.Interfaces;
using Cart.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cart.WebApi.Data.Repository.implements
{
    public class CartRepository (CartContext cartContext) : ICartRepository
    {
        private readonly CartContext _cartContext = cartContext;

        public async Task AddItemAsync(CartItemEntity cartItem, CancellationToken cancellationToken = default)
        {
            await this._cartContext.CartItems.AddAsync(cartItem, cancellationToken);
        }

        public void RemoveItem(CartItemEntity cartItem)
        {
            this._cartContext.CartItems.Remove(cartItem);
        }

        public async Task<List<CartItemEntity>> GetAllAsync(Guid customerId)
        {
            return await this._cartContext.CartItems.Where(x => x.CartId == customerId).ToListAsync();
        }

        public async Task<CartItemEntity?> GetItemAsync(Guid customerId, Guid productId)
        {
            return await this._cartContext.CartItems.FirstOrDefaultAsync(x => x.CartId == customerId && x.ProductId == productId);
        }

        public async Task<CartEntity?> GetCartAsync(Guid customerId)
        {
            return await this._cartContext.Carts.Include(x => x.Items).AsTracking().FirstOrDefaultAsync(x => x.Id == customerId);
        }

        public async Task<CartEntity> CreateCart(CartEntity cartEntity, CancellationToken cancellationToken = default)
        {
            await this._cartContext.Carts.AddAsync(cartEntity, cancellationToken);

            return cartEntity;
        }

        public void Clear(List<CartItemEntity> itens)
        {
            this._cartContext.CartItems.RemoveRange(itens);
        }
    }
}
