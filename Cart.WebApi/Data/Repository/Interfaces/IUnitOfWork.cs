namespace Cart.WebApi.Data.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public ICartRepository CartRepository { get; }
    }
}
