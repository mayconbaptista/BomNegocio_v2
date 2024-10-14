using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class AdvertiserRepository : WriteRepository<AdvertiserEntity>, IAdvertiserRepository
{
    public AdvertiserRepository(BNContext bnContext) : base(bnContext) { }

}
