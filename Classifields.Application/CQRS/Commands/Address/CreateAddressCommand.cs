using Classifields.Application.CQRS.Commands;
using Classifields.Domain.Entities;

namespace Classifields.Application.CQRS.Commands.Address
{
    public class CreateAddressCommand : ICommand<AddressEntity>
    {
        public string Street { get; private set; }
        public uint Number { get; private set; }
        public string Complement { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        public uint ClientId { get; private set; }

        public CreateAddressCommand(
            string street,
            uint number,
            string complement,
            string neighborhood,
            string city,
            string state,
            string country,
            string zipCode,
            uint clientId)
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
            ClientId = clientId;
        }
    }
}
