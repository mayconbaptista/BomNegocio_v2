using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.Api.Data.Repositories
{
    public sealed class ProductRepository(CatalogContext catalogContext)
        : WriteRepository<ProductEntity, Guid>(catalogContext), IProductRepository
    {
        public async Task<ICollection<ProductEntity>> GetByCategoryAsync(Guid? categoryId)
        {
            var query = _context.Products
                .Include(x => x.Category)
                .AsNoTracking();

            if(categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId);

            return await query.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<ICollection<ProductEntity>> GetRange (List<Guid> productIds)
        {
            var query = _context.Products
                .Where(x => productIds.Contains(x.Id))
                .Select(x => new ProductEntity
                {
                    Id = x.Id,
                    SkuCode = x.SkuCode,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                })
                .AsNoTracking();

            return await query.OrderBy(c => c.Name).ToListAsync();
        }

        public override async Task<IList<ProductEntity>> GetAllAsync(Expression<Func<ProductEntity, bool>>? expression = null, bool asTraking = false)
        {
            var query = _context
                .Products
                .Include( x => x.Category)
                .Include(x => x.Images)
                .AsQueryable();

            if (asTraking == false)
                query = query.AsNoTracking();

            if (expression != null)
                query.Where(expression);

            return await query.ToListAsync();
        }

        public async Task<IList<ImageEntity>> GeImages()
        {
            return await _context.Images.AsTracking().ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdWithDetais(Guid id)
        {
            return await _context
                .Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Images)
                .FirstOrDefaultAsync();
        }
    }
}
