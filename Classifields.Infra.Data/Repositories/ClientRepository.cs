using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class ClientRepository : WriteRepository<ClientEntity>, IClientRepository
{
    public ClientRepository(BNContext dbContext) : base(dbContext)
    {
    }
}
