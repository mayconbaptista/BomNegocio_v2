using Classifields.Domain.Entities;
using System.Linq.Expressions;

namespace Classifields.Domain.Interfaces;

public interface IReadRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(uint id);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>>? predicate = null, bool Tracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool Tracking = true);
}
