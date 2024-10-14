namespace Classifields.Domain.Entities
{
    public sealed class EvaluetionEntity : BaseEntity
    {
        public byte Note { get; private set; }
        public string? Commenter { get; private set; }
        public DateTime CreationAt { get; private set; }
        public int AnnouncementId { get; set; }

        /* EF N..1 */
        public AnnouncementEntity Announcement { get; set; }

        public int ClientId { get; set; }
        public ClientEntity Client { get; set; }


        public EvaluetionEntity(byte note, string? commenter, int announcementId, int clientId)
        {
            Note = note;
            Commenter = commenter;
            CreationAt = DateTime.Now;
            AnnouncementId = announcementId;
            ClientId = clientId;
            Validate();
        }

        public override void Validate()
        {
            When(Note < 1 || Note > 5, "Nota inválida.");
            When(Commenter != null && Commenter.Length < 3 || Commenter?.Length > 250, "O campo comentário deve ter de 3 á 250 caracteres.");
            When(CreationAt == default, "Data de criação é inválida.");
            When(AnnouncementId == 0, "Anúncio é inválido.");
            When(ClientId == 0, "Cliente é inválido.");
            Execute();
        }
    }
}
