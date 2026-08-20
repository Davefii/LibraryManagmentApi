using BusinessLayer.DTOs;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class DashbaordService
    {
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly MemberRepository _memberRepository;
        private readonly BorrowingRepository _borrowingRepository;
        private readonly AppDbContext _context;
        public DashbaordService(
            BookRepository bookRepository,
            AuthorRepository authorRepository,
            CategoryRepository categoryRepository,
            MemberRepository memberRepository,
            BorrowingRepository borrowingRepository,
            AppDbContext context
            )
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _memberRepository = memberRepository;
            _borrowingRepository = borrowingRepository;
            _context = context;
        }

        public async Task<DashboardDTO> Dashboard()
        {
            return new DashboardDTO
            {
                TotalBooks = await _bookRepository.TotalBooks(),

                TotalAuthors = await _authorRepository.TotalAuthors(),

                TotalCategories = await _categoryRepository.TotalCategory(),

                TotalMembers = await _memberRepository.TotalMembers(),

                ActiveBorrowings = await _borrowingRepository.TotalBorrowings(),

                OverdueBorrowings = await _borrowingRepository .GetOverdueBorrowingsCountAsync(),

                BooksUnavailable = await _bookRepository.GetUnavailableBooksCountAsync()
            };


        }
        public async Task<List<PopularBookDTO>>GetPopularBooks()
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
        public async Task<List<PopularBookDTO>> GetPopularBooksReturned()
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
        public async Task<List<BooksbycategoryDTO>> GetBooksByCategory()
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

        public async Task<List<RecentborrowingsDTO>> GetRecentborrowingsAsync()
        {
            var Recentborrowings = await _borrowingRepository.Recentborrowings();
            return Recentborrowings.Select(RB => new RecentborrowingsDTO
            {
                BorrowingId = RB.Id,
                MemberName = RB.Member.Name,
                BookTitle = RB.Book.Title,
                Status = RB.IsReturned ? "Returned" : "Active",
                Datee = RB.BorrowDate <= DateTime.Now ? RB.BorrowDate : DateTime.Now,
            }).ToList();
        }
    }
}
