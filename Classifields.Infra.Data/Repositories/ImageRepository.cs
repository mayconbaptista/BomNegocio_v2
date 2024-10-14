using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public class ImageRepository : WriteRepository<ImageEntity>, IImageRepository
{
    public ImageRepository(BNContext dbContext) : base(dbContext)
    {
    }
}
