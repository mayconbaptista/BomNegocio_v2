using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class CategoryRepository : WriteRepository<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(BNContext bnContext) : base(bnContext) { }

}
