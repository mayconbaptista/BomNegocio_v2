
using BuildInBlocks.Messaging.Abstractions;
using BuildInBlocks.Messaging.Dtos;

namespace BuildInBlocks.Messaging.Events
{
    public record OrderCanceledIntegrationEvent(Guid OrderId,List<ItemDto> Items) : IntegrationEvent;
}
