
using BuildInBlocks.Messaging.Abstractions;

namespace BuildInBlocks.Messaging.Dtos;

public record ItemDto(Guid ProductId, uint Quantity) : IntegrationEvent;