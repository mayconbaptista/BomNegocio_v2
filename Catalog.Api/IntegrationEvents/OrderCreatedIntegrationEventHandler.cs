using MediatR;
using BuildInBlocks.Messaging.Events;
using MassTransit;
using Catalog.Api.Data.Interfaces;

namespace Catalog.Api.IntegrationEvents
{
    public class OrderCreatedIntegrationEventHandler(IUnitOfWork unitOfWork, ISender sender, ILogger<OrderCreatedIntegrationEventHandler> logger)
        : IConsumer<OrderCreatedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            try
            {
                logger.LogInformation("IntegrationEvent consumed: {0}", context.Message.EventType);

                var orderitemsids = context.Message.Items.Select(x => x.ProductId).ToList();

                var order = context.Message;

                var productsEntt = await unitOfWork.IProductRepository.GetAllAsync(x => orderitemsids.Contains(x.Id), asTraking:true);

                if(productsEntt.Count() != orderitemsids.Count())
                {
                    var encontrados = productsEntt.Select(x => x.Id);

                    var inexistentes = orderitemsids.Where(x => !encontrados.Contains(x));

                    throw new ApplicationException($"IntegrationEvent: Product not found {inexistentes.ToString()}");
                }

                foreach (var product in productsEntt)
                {
                    var compra = order.Items.FirstOrDefault(x => x.ProductId == product.Id);

                    if (compra != null)
                    {
                        product.Quantity -= compra.Quantity > product.Quantity ? product.Quantity : compra.Quantity;
                    }
                }

                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"message: {ex.Message}");
                throw;
            }
        }
    }
}
