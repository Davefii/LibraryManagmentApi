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
    public class BorrowingRepository
    {
        private readonly AppDbContext _context;
        private readonly BookRepository _bookRepository;

        public BorrowingRepository(AppDbContext context, BookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        public async Task<List<Borrowing>> GetAllAsync()
        {
            return await _context.Borrowings
                .AsNoTracking()
                .Include(book => book.Book)
                .Include(member => member.Member)
                .ThenInclude(user => user.User)
                .ToListAsync();
        }

        public async Task<Borrowing?> GetByIdAsync(int id)
        {
            return await _context.Borrowings
                .Include(book => book.Book)
                .Include(member => member.Member)
                .ThenInclude(user => user.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Borrowing borrowing)
        {
            await _context.Borrowings.AddAsync(borrowing);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Borrowing borrowing)
        {
            _context.Borrowings.Update(borrowing);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Borrowing borrowing)
        {
            _context.Borrowings.Remove(borrowing);

            await _context.SaveChangesAsync();
        }

        public async Task<Borrowing?> GetActiveBorrowingAsync(int memberId,int bookId)
        {
            return await _context.Borrowings
                .FirstOrDefaultAsync(x =>
                    x.MemberId == memberId &&
                    x.BookId == bookId &&
                    !x.IsReturned);
        }
        public async Task<int> CountActiveBorrowingsAsync(int memberId)
        {
            return await _context.Borrowings
                .CountAsync(x =>
                    x.MemberId == memberId &&
                    !x.IsReturned);
        }
        public async Task<List<Borrowing>>GetOverdueBorrowingsAsync()
        {
            return await _context.Borrowings
                    .Include(x => x.Book)
                    .Include(x => x.Member)
                .Where(x =>
                    !x.IsReturned &&
                    x.DueDate < DateTime.UtcNow)
                .ToListAsync();
        }
        public async Task<int> TotalBorrowings()
        {
            return await _context.Borrowings.CountAsync();
        }
        public async Task<int> GetOverdueBorrowingsCountAsync()
        {
            return await _context.Borrowings
                .CountAsync(x =>
                    !x.IsReturned &&
                    x.DueDate < DateTime.UtcNow);
        }

        public async Task<List<Borrowing>>GetBorrowingsByMemberIdAsync(int memberId)
        {
            return await _context.Borrowings
                .Where(x => x.MemberId == memberId)
                .ToListAsync();
        }
        public async Task<List<Borrowing>> GetPopularBooksAsync()
        {
            return await _context.Borrowings.AsNoTracking().Include(b => b.Book).ToListAsync();
        }
        public async Task<List<Borrowing>> GetPopularBooksReturned()
        {
            return await _context.Borrowings.AsNoTracking().Include(b => b.Book).ToListAsync();
        }
        public async Task<List<Borrowing>> Recentborrowings()
        {
            return await _context.Borrowings.AsNoTracking().Include(b => b.Book).Include(M => M.Member).ToListAsync();
        }
    }
}
