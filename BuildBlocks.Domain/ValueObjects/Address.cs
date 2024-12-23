
namespace BuildBlocks.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string Name { get; private set; } 
        public string Street { get; private set; }
        public string City { get; private set; } 
        public string State { get; private set; } 
        public string Country { get; private set; } 
        public string ZipCode { get; private set; } 

        public Address(string name, string street, string city, string state, string country, string zipCode)
        {
            Name = name;
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        public static Address Create(string name, string street, string city, string state, string country, string zipCode)
        {
            return new Address(name, street, city, state, country, zipCode);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
    }
}
