
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.ValueObjects;

public class Customer : ValueObject
{
    public string Email { get; private set; }
    public string Name { get; private set; }

    public Customer(string email, string name)
    {
        Email = email;
        Name = name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return Name;
    }
}
