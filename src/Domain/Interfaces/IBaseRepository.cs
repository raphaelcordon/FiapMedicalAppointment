using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity?> FindAsync(Guid id);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> condition);
    TEntity Update(TEntity entity);
    Task DeleteAsync(Guid id);
    IEnumerable<TEntity> List();
    Task<List<TEntity>> ListAsync();
    Task<int> SaveChangesAsync();
    void Dispose();
    IDbContextTransaction BeginTransaction();
}