using Classifields.Application.CQRS.Commands.Announcement;
using Classifields.Application.CQRS.Handlers;
using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;

namespace Classifields.Application.CQRS.Handlers.Announcements;

public sealed class CreateAnnouncementCommandHandler
    : ICommandHandler<CreateAnnouncementCommand, AnnouncementEntity>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAnnouncementCommandHandler(IUnitOfWork unitOfWork)
    {
        _announcementRepository = unitOfWork.AnnouncementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AnnouncementEntity> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var announcementETT = new AnnouncementEntity(
            request.Title,
            request.Description,
            request.Price,
            request.AdvertiserId,
            request.CategoryId,
            default);

        var result = await _announcementRepository.CreateAsync(announcementETT);
        await _unitOfWork.CommitAsync();

        return result;
    }
}
