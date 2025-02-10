using System.Linq.Expressions;

namespace VNTrackPal.Application.Persistence.Repository;
public interface IRepositoryBase<TEntity, in TKey> where TEntity : class
{
    IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
    Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);
    void Add(TEntity entity);
    void AddRange(List<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void RemoveMultiple(List<TEntity> entities);
}
