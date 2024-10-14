
namespace Classifields.Application.CQRS.Handlers.Announcements;

public sealed class GetAnnouncementsByIdQueryHandler
    : IQueryHandler<GetAnnouncementByIdQuery, AnnouncementEntity>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAnnouncementsByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<AnnouncementEntity> Handle(GetAnnouncementByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.AnnouncementRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Announcement not found");

        throw new NotImplementedException();
    }
}
