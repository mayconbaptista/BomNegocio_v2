
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Events;

namespace Order.Application.CQRS.Order.EventsHandlers.Domain
{
    public class OrderCanceledEventHandler(
        IPublishEndpoint publishEndpoint, ILogger<OrderCanceledEventHandler> logger)
        : INotificationHandler<OrderCanceledEvent>
    {
        public async Task Handle(OrderCanceledEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Order {notification.OrderId} canceled event received");

            await publishEndpoint.Publish(notification, cancellationToken);
        }
    }
}
