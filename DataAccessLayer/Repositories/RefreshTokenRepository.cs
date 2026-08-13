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
    public class RefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            RefreshToken refreshToken)
        {
            await _context.RefreshTokens
                .AddAsync(refreshToken);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            RefreshToken refreshToken)
        {
            _context.RefreshTokens
                .Update(refreshToken);

            await _context.SaveChangesAsync();
        }
        public async Task<List<RefreshToken>>GetByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        /*public async Task<RefreshToken?>GetActiveTokenAsync(int userId)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.RevokedAt == null &&
                    x.ExpiresAt > DateTime.UtcNow);
        }*/
        public async Task<List<RefreshToken>> GetActiveTokensAsync()
        {
            return await _context.RefreshTokens
                .Include(x => x.User)
                .Where(x =>
                    x.RevokedAt == null &&
                    x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
