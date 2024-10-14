using Classifields.Application.CQRS.Handlers;
using Classifields.Application.CQRS.Queryes.Announcement;
using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;

namespace Classifields.Application.CQRS.Handlers.Announcements;

public sealed class GetAllAnnouncementsQueryHandler
    : IQueryHandler<GetAllAnnouncementsQuery, IEnumerable<AnnouncementEntity>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnnouncementRepository _announcementRepository;

    public GetAllAnnouncementsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _announcementRepository = unitOfWork.AnnouncementRepository;
    }

    public async Task<IEnumerable<AnnouncementEntity>> Handle(GetAllAnnouncementsQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.AnnouncementRepository.GetAllAsync();

        return result;
    }
}
