using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.DTOs;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class DashbaordRepositry : IDashboardRepository
    {
        private readonly AppDbContext _context;
        public DashbaordRepositry(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<PopularBookDTO>> GetPopularBooksAsync()
        {
            return await _context.Borrowings.AsNoTracking()
            .GroupBy(b => new { b.BookId, b.Book.Title })
            .Select(g => new PopularBookDTO
            {
                BookId = g.Key.BookId,
                Title = g.Key.Title,
                BorrowCount = g.Count()
            })
            .OrderByDescending(b => b.BorrowCount)
            .Take(5)
            .ToListAsync();
        }
        public async Task<List<PopularBookDTO>> GetPopularBooksReturnedAsync()
        {
            return await _context.Borrowings.AsNoTracking()
            .Where(b => b.IsReturned == true)
            .GroupBy(b => new { b.BookId, b.Book.Title })
            .Select(g => new PopularBookDTO
            {
                BookId = g.Key.BookId,
                Title = g.Key.Title,
                BorrowCount = g.Count()
            })
            .OrderByDescending(b => b.BorrowCount)
            .Take(5)
            .ToListAsync();
        }
        public async Task<List<BooksbycategoryDTO>> GetBooksByCategoryAsync()
        {
            return await _context.Categories.AsNoTracking()
                .Include(B => B.Books)
                .GroupBy(b => new { b.Name })
                .Select(C => new BooksbycategoryDTO
                {
                    NameCategory = C.Key.Name,
                    TotalBooks = C.SelectMany(c => c.Books).Count()
                })
                .Where(T => T.TotalBooks >= 1)
                .OrderByDescending(C => C.TotalBooks)
                .ToListAsync();
        }
    }
}
