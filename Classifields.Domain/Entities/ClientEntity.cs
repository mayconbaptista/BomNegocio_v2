namespace Classifields.Domain.Entities
{
    public sealed class ClientEntity : BaseEntity
    {
        public uint UserId { get; init; }
        public DateOnly RegistrationDate { get; init; }
        public DateOnly? DeactivationDate { get; private set; }

        /* 1 .. 1 */
        public UserEntity User { get; init; }

        /* 1 .. N */
        public List<EvaluetionEntity> Evaluetions { get; } = new();
        public List<WisheEntity> Wishes { get; } = new();

        public ClientEntity(uint userId)
        {
            UserId = userId;
            RegistrationDate = new DateOnly();
            DeactivationDate = null;
            Validate();
        }

        public override void Validate()
        {
            When(UserId == 0, "O Id do usuário é inválido.");
            When(RegistrationDate == default, "Data de registro é inválida.");
            When(DeactivationDate != null && DeactivationDate < RegistrationDate, "Data de desativação não pode ser menor que a data de ativação");
            Execute();
        }

        public void Deactivate(DateOnly deactivationDate)
        {
            DeactivationDate = deactivationDate;
            When(DeactivationDate == default, "Data de desativação é inválida.");
            Validate();
        }
    }
}
