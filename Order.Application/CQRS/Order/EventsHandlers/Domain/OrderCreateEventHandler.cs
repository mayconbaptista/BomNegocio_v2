
using BuildInBlocks.Messaging.Dtos;
using BuildInBlocks.Messaging.Events;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Events;

namespace Order.Application.CQRS.Order.EventsHandlers.Domain
{
    public class OrderCreateEventHandler
        (IPublishEndpoint publishEndpoint, ILogger<OrderCreateEventHandler> logger)
        : INotificationHandler<OrderCreateEvent>
    {

        public async Task Handle(OrderCreateEvent notification, CancellationToken cancellationToken)
        {
            try
            {

                logger.LogInformation("DomainEvent consumed: {0} - eventId {1} - OccurredOn: {2} - OrderId: {3}", notification.EventType, notification.EventId, notification.OccurredOn, notification.Order.Id);

                var itens = notification.Order.OrderItems.Select(x => new ItemDto(x.ProductId, x.Quantity)).ToList();

                var orderCreatedIntegrationEvent = new OrderCreatedIntegrationEvent(notification.Order.Id, itens);

                logger.LogInformation("IntegrationEvent Publish: {0} - eventId {1} - CreateAt: {2} - OrderId: {3}", orderCreatedIntegrationEvent.EventType, orderCreatedIntegrationEvent.Guid, orderCreatedIntegrationEvent.CreationAt, notification.Order.Id);

                await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
