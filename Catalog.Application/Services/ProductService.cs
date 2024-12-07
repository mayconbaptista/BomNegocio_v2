
namespace Catalog.Application.Services
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ProductModel> GetById(uint id)
        {
            var product = await this._unitOfWork
                .IProductRepository
                .FindAsync(x => x.Id == id) ?? throw new Exception("Product not found");

            return product;
        }

        public async Task<ProductModel> Save (ProductModel product)
        {
            product.Validate();

            await _unitOfWork.IProductRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return product;
        }

        public async Task<IList<ProductModel>> GetAll()
        {
            return await _unitOfWork.IProductRepository.GetAllAsync();
        }

        public async Task<IList<ProductModel>> GetAll(int categoryId)
        {
            return await this._unitOfWork.IProductRepository.GetAllAsync(x => x.CategoryId == categoryId);
        }
    }
}
