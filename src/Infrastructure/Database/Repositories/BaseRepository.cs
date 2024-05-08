using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Database.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, ISoftDeletable
{
    protected readonly DatabaseContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public BaseRepository(DatabaseContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public async Task<TEntity?> FindAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> condition)
    {
        return DbSet.Where(condition);
    }

    public TEntity Update(TEntity entity)
    {
        DbSet.Update(entity);
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            if (entity is ISoftDeletable softDeletable)
            {
                softDeletable.IsActive = false;
                DbSet.Update(entity);
            }
            else
            {
                DbSet.Remove(entity);
            }
            await SaveChangesAsync();
        }
    }

    public IEnumerable<TEntity> List()
    {
        return DbSet.AsNoTracking().ToList();
    }

    public async Task<List<TEntity>> ListAsync()
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            return await DbSet.AsNoTracking().Where(x => ((ISoftDeletable)(object)x).IsActive).ToListAsync();
        }
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }
}
