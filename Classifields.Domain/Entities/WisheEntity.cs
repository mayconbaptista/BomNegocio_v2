namespace Classifields.Domain.Entities
{
    public sealed class WisheEntity : BaseEntity
    {
        public uint ClientId { get; private set; }
        public uint AnnouncementId { get; private set; }

        /* 1..N */
        public ClientEntity Client { get; set; }
        public AnnouncementEntity Announcement { get; set; }

        public WisheEntity(uint clientId, uint announcementId)
        {
            ClientId = clientId;
            AnnouncementId = announcementId;
            Validate();
        }

        public override void Validate()
        {
            When(ClientId == 0, "Cliente é inválido.");
            When(AnnouncementId == 0, "Anúncio é inválido.");
            Execute();
        }
    }
}
