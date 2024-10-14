namespace Classifields.WebAPI.Requests.Address;

public record CreateAddressRequest(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode);
