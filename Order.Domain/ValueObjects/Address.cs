
using BuildBlocks.Domain.Abstractions;
using Order.Domain.Exceptions;
using Order.Domain.Extensions;

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
            List<string> errors = new();

            string zipCodeClear = zipCode.Replace(@"^[0-9]", string.Empty);

            errors.AddIf(string.IsNullOrEmpty(name), "O nome do destinatário é obrigatório")
                .AddIf(name.Length < 10 || name.Length > 100, "O nome do destinatário deve conter entre 10 e 100 caracteres")
                .AddIf(string.IsNullOrEmpty(street), "O nome da rua é obrigatório")
                .AddIf(street.Length < 10 || street.Length > 100, "O nome da rua deve conter entre 10 e 100 caracteres")
                .AddIf(string.IsNullOrEmpty(city), "O nome da cidade é obrigatório")
                .AddIf(city.Length < 2 || city.Length > 100, "O nome da cidade deve conter entre 2 e 100 caracteres")
                .AddIf(string.IsNullOrEmpty(state), "O nome do estado é obrigatório")
                .AddIf(state.Length != 2, "O sigla do estado deve conter 2 caracteres")
                .AddIf(string.IsNullOrEmpty(country), "O nome do país é obrigatório")
                .AddIf(country.Length < 2 || country.Length > 100, "O nome do país deve conter entre 2 e 100 caracteres")
                .AddIf(zipCodeClear.Length != 8, "O código postal inválido");

            DomainException.ThrowIfAnyErro(errors, "Dados do endereçõ inválidos");


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
