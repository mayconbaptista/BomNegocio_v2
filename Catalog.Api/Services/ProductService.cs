using Catalog.Api.Data.Interfaces;
using Grpc.Core;

namespace Catalog.Api.Services
{
    public class ProductService(ILogger<ProductService> logger, IUnitOfWork unitOfWork) : ProductGRPC.ProductGRPCBase
    {
        private readonly ILogger<ProductService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public override async Task GetProducts(
            IAsyncStreamReader<ProductRequest> requestStream,
            IServerStreamWriter<ProductReply> responseStream,
            ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("GetProducts method GRPC called");

                var listIds = new List<Guid>();

                await foreach (var item in requestStream.ReadAllAsync())
                {
                    if(!Guid.TryParse(item.Id, out var productId))
                    {
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "O identificador do produto é inválido"));
                    }

                    listIds.Add(productId);
                };

                var products = await _unitOfWork.IProductRepository.GetRange(listIds);

                foreach (var product in products)
                {
                    var element = new ProductReply
                    {
                        Id = product.Id.ToString(),
                        SkuCode = product.SkuCode,
                        Price = (float)product.Price,
                        Quantity = product.Quantity,
                    };

                    await responseStream.WriteAsync(element);
                }

                await Task.CompletedTask;
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "RpcException");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProducts method GRPC failed");
                throw;
            }
        }
    }
}
