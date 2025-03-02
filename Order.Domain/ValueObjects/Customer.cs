
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.ValueObjects;

public class Customer : ValueObject
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }

    public Customer(Guid id, string email, string name)
    {
        Id = id;
        Email = email;
        Name = name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Email;
        yield return Name;
    }
}
