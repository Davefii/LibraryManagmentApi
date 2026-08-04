using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors
                .AsNoTracking().Include(B => B.Books)
                .ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors
                .FindAsync(id);
        }

        public async Task<bool> ExistsAuthorAsync(int id)
        {
            return await _context.Authors
                .AnyAsync(a => a.Id == id);
        }

        public async Task<Author?> GetAuthorByNameAsync(string AuthorName)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.FullName == AuthorName);
        }

        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);

            await _context.SaveChangesAsync();
        }
        public async Task<int> TotalAuthors()
        {
            return await _context.Authors.CountAsync();
        }
    }
}
