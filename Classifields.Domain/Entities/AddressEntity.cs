namespace Classifields.Domain.Entities;

public sealed class AddressEntity : BaseEntity
{
    public string Street { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }
    public uint Number { get; private set; }

    /* N .. 1 */
    public uint? UserId { get; init; }
    public UserEntity? User { get; set; }

    /* 1..1 */
    public uint? AnnouncementId { get; init; }
    public AnnouncementEntity? Announcement { get; set; }


    #region "Constructors"
    public AddressEntity(
        string street,
        string complement,
        string neighborhood,
        string city,
        string state,
        string zipCode,
        string country,
        uint number,
        uint? userId,
        uint? announcementId)
    {
        Street = street;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        Number = number;
        UserId = userId;
        AnnouncementId = announcementId;
        Validate();
    }
    #endregion

    #region "validator"
    public override void Validate()
    {
        When(string.IsNullOrWhiteSpace(Street), "Rua é inválida.");
        When(Street.Length < 3 || Street.Length > 60, "O campo rua deve ter de 3 á 60 caracteres.");
        When(string.IsNullOrWhiteSpace(Complement), "Complemento é inválido.");
        When(Complement.Length < 3 || Complement.Length > 60, "O campo complemento deve ter de 3 á 60 caracteres.");
        When(string.IsNullOrWhiteSpace(Neighborhood), "Bairro é inválido.");
        When(Neighborhood.Length < 3 || Neighborhood.Length > 60, "O campo bairro deve ter de 3 á 60 catacteres.");
        When(string.IsNullOrWhiteSpace(City), "Cidade é inválida.");
        When(City.Length < 3 || City.Length > 60, "O campo cidade deve ter de 3 á 60 caracteres.");
        When(string.IsNullOrWhiteSpace(State), "Estado é inválido.");
        When(State.Length != 2, "O campo estado deve ter 2 caracteres.");
        When(string.IsNullOrWhiteSpace(Country), "País é inválido.");
        When(Country.Length < 3 || Country.Length > 60, "O campo país deve ter de 3 á 60 caracteres.");
        When(string.IsNullOrWhiteSpace(ZipCode), "CEP é inválido.");
        When(ZipCode.Length != 8, "O campo CEP deve ter 8 caracteres.");
        When(Number == 0, "Número é inválido.");
        When(UserId is null && AnnouncementId is null, "O Endereço deve ser atribuido a um usuário e/ou anuncio.");
        When(UserId is not null && UserId == 0, "Id do usuário é inválido.");
        When(AnnouncementId is not null && AnnouncementId == 0, "Id do anúncio é inválido.");
        Execute();
    }
    #endregion
}
