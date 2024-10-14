using Classifields.Domain.Entities;

namespace Classifields.Domain.Interfaces
{
    public interface IAnnouncementRepository : IWriteRepository<AnnouncementEntity>
    {
        Task<object?> GetByIdAsync(int id);
    }
}
