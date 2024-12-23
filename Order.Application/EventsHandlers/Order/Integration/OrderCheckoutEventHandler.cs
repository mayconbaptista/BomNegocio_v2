using BuildInBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Order.Application.EventsHandlers.Order.Integration
{
    public class OrderCheckoutEventHandler(ISender sender, ILogger<OrderCheckoutEventHandler> logger) 
        : IConsumer<CartCheckoutEvent>
    {
        public Task Consume(ConsumeContext<CartCheckoutEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
