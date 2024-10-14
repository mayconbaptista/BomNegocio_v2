namespace Classifields.Domain.Entities;

public sealed class ImageEntity : BaseEntity
{
    public string Path { get; private set; }
    public uint AnnouncementId { get; init; }
    public uint UserId { get; init; }

    /* 1..1 */
    public AnnouncementEntity Announcement { get; set; }
    public UserEntity User { get; set; }

    public ImageEntity(string path, uint announcementId, uint userId)
    {
        Path = path;
        AnnouncementId = announcementId;
        UserId = userId;
        Validate();
    }

    public void UpdatePath(string path)
    {
        Path = path;
        Validate();
    }

    public override void Validate()
    {
        When(Path.Length < 3 || Path.Length > 60, "O caminho da imagem deve ter de 3 á 60 caracteres.");
        When(AnnouncementId == 0, "Anúncio é inválido.");
        When(UserId == 0, "Usuário é inválido.");
        Execute();
    }
}

