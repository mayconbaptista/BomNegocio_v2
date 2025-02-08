namespace Catalog.Api.Data.Interfaces
{
    public interface IUnitOfWork
    {
        public IProductRepository IProductRepository { get; }
        public ICategoryRepository ICategoryRepository { get; }
        public IImageRepository IImageRepository { get; }

        public int SaveChanges();
        public ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
