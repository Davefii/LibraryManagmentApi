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
    public class MemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _context.Members.Include(m => m.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Member?> GetByUserIdAsync(int userId)
        {
            return await _context.Members
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Member member)
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }
        public async Task<int> TotalMembers()
        {
            return await _context.Members.CountAsync();
        }
    }
}
