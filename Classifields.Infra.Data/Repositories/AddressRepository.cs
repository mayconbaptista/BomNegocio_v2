using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class AddressRepository : WriteRepository<AddressEntity>, IAddressRepository
{
    public AddressRepository(BNContext dbContext) : base(dbContext)
    {

    }
}
