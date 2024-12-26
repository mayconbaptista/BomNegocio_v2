
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Order.Domain.Events;

namespace Order.Application.CQRS.Order.EventsHandlers.Domain
{
    public class OrderCreateEventHandler
        (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreateEventHandler> logger)
        : INotificationHandler<OrderCreateEvent>
    {
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public readonly IFeatureManager _featureManager = featureManager;
        public readonly ILogger<OrderCreateEventHandler> _logger = logger;

        public async Task Handle(OrderCreateEvent notification, CancellationToken cancellationToken)
        {
            if(await _featureManager.IsEnabledAsync("OrderFullFilment"))
            {
                await _publishEndpoint.Publish(notification.Adapt<OrderDto>(), cancellationToken);
            }
        }
    }
}
