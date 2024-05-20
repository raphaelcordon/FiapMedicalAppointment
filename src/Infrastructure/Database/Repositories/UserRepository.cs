using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : IBaseRepository<UserProfile>
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProfile>> ListAsync()
        {
            return await _context.UserProfiles
                .Include(u => u.MedicalSpecialties)
                .ToListAsync();
        }

        public async Task<UserProfile?> FindAsync(Guid id)
        {
            return await _context.UserProfiles
                .Include(u => u.MedicalSpecialties)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<UserProfile> FindByCondition(Expression<Func<UserProfile, bool>> expression)
        {
            return _context.UserProfiles
                .Include(u => u.MedicalSpecialties)
                .Where(expression);
        }

        public async Task AddAsync(UserProfile entity)
        {
            await _context.UserProfiles.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(UserProfile entity)
        {
            _context.UserProfiles.Update(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await FindAsync(id);
            if (entity != null)
            {
                _context.UserProfiles.Remove(entity);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}