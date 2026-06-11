using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserProfile>> GetAllAsync()
        {
            return await _context.UserProfiles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserProfile?> GetByIdAsync(int id)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(UserProfile profile)
        {
            await _context.UserProfiles.AddAsync(profile);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserProfile profile)
        {
            _context.UserProfiles.Remove(profile);

            await _context.SaveChangesAsync();
        }
    }
}
