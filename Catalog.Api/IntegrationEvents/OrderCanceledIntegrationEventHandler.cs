using BuildInBlocks.Messaging.Events;
using Catalog.Api.Data.Interfaces;
using MassTransit;
using MediatR;

namespace Catalog.Api.IntegrationEvents
{
    public class OrderCanceledIntegrationEventHandler(IUnitOfWork unitOfWork, ISender sender, ILogger<OrderCanceledIntegrationEventHandler> logger)
        : IConsumer<OrderCanceledIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<OrderCanceledIntegrationEvent> context)
        {
            try
            {
                logger.LogInformation("IntegrationEvent consumed: {0} - RecevedAt: {1}", context.Message.EventType, context.Message.CreationAt);

                var orderitemsids = context.Message.Items.Select(x => x.ProductId).ToList();

                var order = context.Message;

                var productsEntt = await unitOfWork.IProductRepository.GetAllAsync(x => orderitemsids.Contains(x.Id), asTraking: true);

                if (productsEntt.Count() != orderitemsids.Count())
                {
                    var encontrados = productsEntt.Select(x => x.Id);

                    var inexistentes = orderitemsids.Where( x => !encontrados.Contains(x));

                    throw new ApplicationException($"IntegrationEvent: {context.Message.EventType} - Product not found {inexistentes.ToString()}");
                }

                foreach (var product in productsEntt)
                {
                    var itemCancelado = order.Items.FirstOrDefault(x => x.ProductId == product.Id);

                    if (itemCancelado != null)
                    {
                        product.Quantity +=  itemCancelado.Quantity;
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
