using Cart.WebApi.Entities;

namespace Cart.WebApi.Data.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<CartEntity> CreateCart(CartEntity cartEntity, CancellationToken cancellationToken = default);
        Task AddItemAsync(CartItemEntity cartItem, CancellationToken cancellationToken = default);
        void RemoveItem(CartItemEntity cartItem);
        Task<CartItemEntity?> GetItemAsync(Guid customerId, Guid productId);
        Task<List<CartItemEntity>> GetAllAsync(Guid customerId);
        Task<CartEntity?> GetCartAsync(Guid customerId);
        void Clear(List<CartItemEntity> itens);
    }
}
