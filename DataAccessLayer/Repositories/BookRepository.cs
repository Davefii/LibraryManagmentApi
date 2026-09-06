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
    public class BookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get All Books
        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .AsNoTracking()
                .Include(x => x.Authors)
                .Include(C => C.Categories)
                .ToListAsync();
        }
        public async Task<List<Book>> GetAllByCategoryAsync(int categoryId)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(x => x.Authors)
                .Include(c => c.Categories)
                .Where(b => b.Categories.Any(c => c.Id == categoryId))
                .ToListAsync();
        }
        public async Task<List<Book>> GetAllByAuthorAsync(int authorId)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(x => x.Authors)
                .Include(c => c.Categories)
                .Where(b => b.Authors.Any(a => a.Id == authorId))
                .ToListAsync();
        }
        // Get Book By Id
        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(x => x.Authors)
                .Include(C => C.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        // Get Book By title
        public async Task<Book?> GetByTitleAsync(string title)
        {
            return await _context.Books
                .Include(x => x.Authors)
                .Include(C => C.Categories)
                .FirstOrDefaultAsync(b => b.Title == title);
        }
        // Get ISBN By title
        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Books
                .Include(x => x.Authors)
                .Include(C => C.Categories)
                .FirstOrDefaultAsync(b => b.Isbn == isbn);
        }
        // Add Book
        public async Task<int> AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        // Update Book
        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);

            await _context.SaveChangesAsync();
        }

        // Delete Book
        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
        }
        public async Task<int> TotalBooks()
        {
            return await _context.Books.CountAsync();
        }
        public async Task<int> TotalBookByCategorie(int CategorieID)
        {
            return await _context.Books
                .Where(bc => bc.Categories.Any(c => c.Id == CategorieID))
                .CountAsync();
        }
        public async Task<int> TotalBookByAuthor(int AuthorID)
        {
            return await _context.Books
                .Where(ba => ba.Authors.Any(a => a.Id == AuthorID))
                .CountAsync();
        }
        public async Task<int> GetUnavailableBooksCountAsync()
        {
            return await _context.Books
                .CountAsync(x => !x.IsAvailable);
        }
        public async Task<List<Book>> GetPopularBooks()
        {
            return await _context.Books
                .Include(b => b.Borrowings)
                .Include(a => a.Authors)
                .Include(c => c.Categories)
                .OrderByDescending(b => b.Borrowings.Count)
                .Take(4)
                .ToListAsync();
        }
    }
}
