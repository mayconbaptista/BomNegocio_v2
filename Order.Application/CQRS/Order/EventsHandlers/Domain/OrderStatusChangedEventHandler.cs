using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Events;
using Order.Domain.Enums;
using BuildInBlocks.Messaging.Dtos;
using BuildInBlocks.Messaging.Events;

namespace Order.Application.CQRS.Order.EventsHandlers.Domain
{
    public class OrderStatusChangedEventHandler
        (IPublishEndpoint publishEndpoint, ILogger<OrderStatusChangedEventHandler> logger)
        : INotificationHandler<OrderStatusChangedEvent>
    {
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public readonly ILogger<OrderStatusChangedEventHandler> _logger = logger;

        public async Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("DomainEvent consumed: {0} - eventId {1} - OccurredOn: {2} - OrderId: {3} - OrderStatus: {4}", notification.EventType, notification.EventId, notification.OccurredOn, notification.Order.Id, notification.Order.Status.ToString());


            if (notification.Order.Status == OrderStatus.Canceled)
            {
                // send order canceled event
                var items = notification.Order.OrderItems.Select(x => new ItemDto(x.ProductId, x.Quantity)).ToList();

                var orderCanceledEvent = new OrderCanceledIntegrationEvent(notification.Order.Id, items);

                await this._publishEndpoint.Publish(orderCanceledEvent, cancellationToken);
            }
        }
    }
}
