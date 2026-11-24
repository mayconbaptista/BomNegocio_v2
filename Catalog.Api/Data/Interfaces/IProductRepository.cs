using Catalog.Api.Entities;
using System.Linq.Expressions;

namespace Catalog.Api.Data.Interfaces
{
    public interface IProductRepository : IWriteRepository<ProductEntity, Guid>
    {
        public Task<ICollection<ProductEntity>> GetByCategoryAsync(Guid? categoryId);

        public Task<ICollection<ProductEntity>> GetRange(List<Guid> productIds);

        public Task<IList<ImageEntity>> GeImages();

        Task<ProductEntity?> GetByIdWithDetais(Guid id);

        Task<List<ProductEntity>> Filter(List<Expression<Func<ProductEntity, bool>>> expressions);
    }
}
