
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Events;

namespace Order.Application.CQRS.Order.EventsHandlers.Domain
{
    public class OrderUpdateEventHandler
        (IPublishEndpoint publishEndpoint, ILogger<OrderUpdateEventHandler> logger)
        : INotificationHandler<OrderStatusChangedEvent>
    {
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public readonly ILogger<OrderUpdateEventHandler> _logger = logger;

        public async Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"Order {notification.OrderId} status changed {notification.OrderStatus.ToString()} event received");

            await this._publishEndpoint.Publish(notification.Adapt<OrderUpdateStatusDto>(), cancellationToken);
        }
    }
}
