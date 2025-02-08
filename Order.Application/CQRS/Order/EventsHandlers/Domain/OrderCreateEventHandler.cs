
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
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public readonly ILogger<OrderCreateEventHandler> _logger = logger;

        public async Task Handle(OrderCreateEvent notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification.Adapt<OrderDto>(), cancellationToken);
        }
    }
}
