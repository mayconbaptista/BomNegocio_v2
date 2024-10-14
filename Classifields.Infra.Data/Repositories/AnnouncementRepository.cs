using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class AnnouncementRepository : WriteRepository<AnnouncementEntity>, IAnnouncementRepository
{
    public AnnouncementRepository(BNContext bnContext) : base(bnContext) { }

    public Task<object?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
