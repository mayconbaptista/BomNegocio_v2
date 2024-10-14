using Classifields.Application.CQRS.Queryes;
using Classifields.Domain.Entities;

namespace Classifields.Application.CQRS.Queryes.Announcement;

public class GetAllAnnouncementsQuery : IQuery<IEnumerable<AnnouncementEntity>>
{

}
