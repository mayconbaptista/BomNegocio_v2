
namespace Catalog.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IProductRepository IProductRepository { get; }
        public ICategoryRepository ICategoryRepository { get; }
        public IImageRepository IImageRepository { get; }

        public int Commit();
        public ValueTask<int> CommitAsync();
    }
}
