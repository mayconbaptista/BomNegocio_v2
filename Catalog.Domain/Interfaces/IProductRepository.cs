
namespace Catalog.Domain.Interfaces
{
    public interface IProductRepository : IWriteRepository<ProductModel, uint>
    {
        public Task<IEnumerable<ProductModel>> GetByCategoryAsync(uint categoryId);
    }
}
