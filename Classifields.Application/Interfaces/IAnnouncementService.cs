using Classifields.Application.DTO;

namespace Classifields.Application.Interfaces;
public interface IAnnouncementService
{
    Task<IEnumerable<AnnouncementDto>> GetAllAsync();
    Task<AnnouncementDto> GetAsync(int id);
    Task DeleteAsync(int id);
    Task<AnnouncementDto> Update(AnnouncementDto newAnnouncement);
    Task<AnnouncementDto> Create(AnnouncementDto newAnnouncement);
}
