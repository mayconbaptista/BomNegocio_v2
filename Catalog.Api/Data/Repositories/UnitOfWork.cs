

using Catalog.Api.Data.Interfaces;

namespace Catalog.Api.Data.Repositories
{
    public class UnitOfWork(CatalogContext context) : IUnitOfWork
    {
        private readonly CatalogContext _context = context;

        private IProductRepository? _productRepository;
        private ICategoryRepository? _categoryRepository;
        private IImageRepository? _imageRepository;

        public IProductRepository IProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        public ICategoryRepository ICategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }

        public IImageRepository IImageRepository
        {
            get
            {
                if (_imageRepository == null)
                {
                    _imageRepository = new ImageRepository(_context);
                }
                return _imageRepository;
            }
        }


        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }

        public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await this._context.SaveChangesAsync(cancellationToken);
        }
    }
}
