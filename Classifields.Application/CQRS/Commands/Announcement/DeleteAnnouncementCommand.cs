using Classifields.Domain.Entities;

namespace Classifields.Application.CQRS.Commands.Announcement
{
    public class DeleteAnnouncementCommand : IRequest<AnnouncementEntity>
    {
        public int Id { get; set; }

        public DeleteAnnouncementCommand(int id)
        {
            Id = id;
        }
    }
}
