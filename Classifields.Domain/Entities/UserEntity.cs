namespace Classifields.Domain.Entities
{
    public sealed class UserEntity : BaseEntity
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Cpf { get; init; }

        #region "relaçoes"
        /* 1..1 */
        public AdvertiserEntity Advertiser { get; private set; }
        public ClientEntity Client { get; private set; }
        public AnnouncementEntity Announcement { get; private set; }

        /* 1..N */
        public readonly List<AddressEntity> Addresses = new();
        public readonly List<ImageEntity> Images = new();
        #endregion

        public UserEntity()
        {
        }
        public UserEntity(string name, string email, string cpf)
        {
            Name = name;
            Email = email;
            Cpf = cpf;
            Validate();
        }

        public override void Validate()
        {
            When(string.IsNullOrEmpty(Name), "O nome não pode ser vazio ou nulo");
            When(Name.Length < 3 || Name.Length > 100, "O nome deve ter entre 3 e 100 caracteres.");
            When(string.IsNullOrEmpty(Email), "Email é inválido.");
            When(!Email.Contains("@"), "Email é inválido.");
            When(Email.Length < 10 || Email.Length > 100, "Email deve ter entre 10 e 100 caracteres.");
            When(string.IsNullOrEmpty(Cpf), "Cpf é inválido.");
            When(Cpf.Length != 11, "Cpf deve ter 11 caracteres.");
            When(Cpf.Any(c => !char.IsDigit(c)), "Cpf deve conter apenas números.");
            Execute();
        }
    }
}
