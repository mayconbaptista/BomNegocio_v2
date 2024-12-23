
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Entities
{
    public sealed class CustomerEntity : BaseEntity<Guid>
    {
        public string Email { get; init; }
        public string Name { get; init; }

        public CustomerEntity(Guid id, string email, string name)
        {
            Id = id;
            Email = email;
            Name = name;
        }
    }
}
