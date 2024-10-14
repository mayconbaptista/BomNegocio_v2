using Classifields.Application.CQRS.Queryes;
using Classifields.Domain.Entities;

namespace Classifields.Application.CQRS.Queryes.Announcement;

public class GetAnnouncementByIdQuery : IQuery<AnnouncementEntity>
{
    public int Id { get; set; }

    public GetAnnouncementByIdQuery(int id)
    {
        Id = id;
    }
}
