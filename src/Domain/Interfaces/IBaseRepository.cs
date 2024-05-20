using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> ListAsync();
        Task<TEntity?> FindAsync(Guid id);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression); // Updated return type
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity); // Ensure this method is declared
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}