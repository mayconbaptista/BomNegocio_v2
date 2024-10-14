using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class EvaluetionRepository : WriteRepository<EvaluetionEntity>, IEvaluetionRepository
{
    public EvaluetionRepository(BNContext dbContext) : base(dbContext)
    {
    }
}
