namespace Classifields.Domain.Entities
{

    public sealed class AdvertiserEntity : BaseEntity
    {
        public int UserId { get; init; }
        public DateTime CreationAt { get; init; }
        public DateTime? DeactivationAt { get; private set; }

        public UserEntity User { get; set; }

        /* 1..N */
        public ICollection<AnnouncementEntity>? Announcements { get; set; }
        public ICollection<AddressEntity>? Enderecos { get; set; }

        public AdvertiserEntity(int userId)
        {
            UserId = userId;
            CreationAt = DateTime.Now;
            DeactivationAt = null;
            Validate();
        }

        public void Deactivate(DateTime deactivationDate)
        {
            DeactivationAt = deactivationDate;
            When(deactivationDate == default, "Data de desativação é inválida.");
            Validate();
        }

        public override void Validate()
        {
            When(UserId == 0, "O Id do usuário é inválido.");
            When(CreationAt == default, "Data de registro é inválida.");
            When(DeactivationAt != null && DeactivationAt < CreationAt, "Data de desativação não pode ser menor que a data de ativação");
            Execute();
        }
    }
}
