using Cart.WebApi.Data.Repository.Interfaces;

namespace Cart.WebApi.Data.Repository.implements
{
    public sealed class UnitOfWork(CartContext cartContext) : IUnitOfWork
    {
        private readonly CartContext _cartContext = cartContext;

        private ICartRepository? _cartRepository;
        public ICartRepository CartRepository 
        {
            get
            {
                return _cartRepository = _cartRepository ?? new CartRepository(_cartContext);
            }
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _cartContext.SaveChangesAsync(cancellationToken);
        }
    }
}
