using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Interfaces
{
    public interface IWriteRepository<TModel> : IReadRepository<TModel> where TModel : BaseModel
    {
        public Task<TModel> AddAsync(TModel model);
        public void Update(TModel model);
        public void Delete (TModel model);
    }
}
