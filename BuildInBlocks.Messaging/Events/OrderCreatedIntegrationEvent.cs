
using BuildInBlocks.Messaging.Abstractions;
using BuildInBlocks.Messaging.Dtos;

namespace BuildInBlocks.Messaging.Events;

public record OrderCreatedIntegrationEvent(Guid OrderId, List<ItemDto> Items) : IntegrationEvent;