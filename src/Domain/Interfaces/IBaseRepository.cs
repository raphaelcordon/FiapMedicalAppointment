using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity?> FindAsync(Guid id);
    Task<TEntity?> FindAsyncByFilter(Expression<Func<TEntity, bool>> filter);
    TEntity Update(TEntity entity);
    Task DeleteAsync(Guid id);
    IEnumerable<TEntity> List();
    Task<int> SaveChangesAsync();
    void Dispose();
}