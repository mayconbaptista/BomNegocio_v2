using AutoMapper;
using Classifields.Application.DTO;
using Classifields.Application.Interfaces;

namespace Classifields.Application.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AnnouncementService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public Task<AnnouncementDto> Create(AnnouncementDto newAnnouncement)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
        {
            var annoucementsQuery = new GetAllAnnouncementsQuery();

            var resultEntity = await _mediator.Send(annoucementsQuery);

            throw new NotImplementedException();

        }

        public Task<AnnouncementDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AnnouncementDto> Update(AnnouncementDto newAnnouncement)
        {
            throw new NotImplementedException();
        }
    }
}
