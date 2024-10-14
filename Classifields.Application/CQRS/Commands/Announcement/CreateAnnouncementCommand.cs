using Classifields.Application.CQRS.Commands;
using Classifields.Domain.Entities;

namespace Classifields.Application.CQRS.Commands.Announcement;

public class CreateAnnouncementCommand : ICommand<AnnouncementEntity>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int AdvertiserId { get; private set; }
    public int CategoryId { get; private set; }
}
