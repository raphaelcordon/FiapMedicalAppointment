using System.Linq.Expressions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Database.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
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

    public async Task<TEntity?> FindAsyncByFilter(Expression<Func<TEntity, bool>> filter)
    {
        return await DbSet.Where(filter).FirstOrDefaultAsync();
    }

    public TEntity Update(TEntity entity)
    {
        DbSet.Update(entity);
        return entity;
    }
    
    public IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }

    public async Task<object> ListAsync()
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            return await DbSet.AsNoTracking()
                .Where(x => ((ISoftDeletable)(object)x).IsActive)
                .ToListAsync();
        }

        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                ((ISoftDeletable)entity).IsActive = false;
                DbSet.Update(entity);
            }
            else
            {
                DbSet.Remove(entity);
            }
        }
    }

    public IEnumerable<TEntity> List()
    {
        return DbSet.AsNoTracking().AsQueryable();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}