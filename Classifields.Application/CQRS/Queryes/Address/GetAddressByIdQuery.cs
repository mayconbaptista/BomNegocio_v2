using Classifields.Domain.Entities;

namespace Classifields.Application.CQRS.Queryes.Address;

internal class GetAddressByIdQuery(uint id) : IQuery<AddressEntity>
{
    public uint Id { get; init; } = id;
}
