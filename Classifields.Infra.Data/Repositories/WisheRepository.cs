using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class WisheRepository : WriteRepository<WisheEntity>, IWisheRepository
{
    public WisheRepository(BNContext dbContext) : base(dbContext)
    {
    }
}

