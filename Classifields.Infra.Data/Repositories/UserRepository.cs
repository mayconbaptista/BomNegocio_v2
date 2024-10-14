using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class UserRepository : WriteRepository<UserEntity>, IUserRepository
{
    public UserRepository(BNContext dbContext) : base(dbContext) { }
}
