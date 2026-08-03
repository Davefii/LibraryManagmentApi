using BusinessLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;

        public BookService(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<List<BookResponseDTO>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllAsync();

            return books.Select(book => new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                CopiesCount= book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CoverImage = book.CoverImage,
                Authors = book.Authors.Select(a => new AuthorSummaryDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                }).ToList(),
                Categories = book.Categories.Select(c => new CategorySummaryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
            }).ToList();
        }
        public async Task<BookForReadOnlyDTO?> GetBookById(int id)
        {
            //return await _bookRepository.GetByIdAsync(id);
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                return null;
            return new BookForReadOnlyDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CoverImage = book.CoverImage,
                Authors = book.Authors.Select(a => new AuthorSummaryDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                }).ToList(),
                Categories = book.Categories.Select(c => new CategorySummaryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
        }
        public async Task<BookForReadOnlyDTO?> GetBookByTitle(string title)
        {
            var book = await _bookRepository.GetByTitleAsync(title);
            if (book == null)
                return null;
            return new BookForReadOnlyDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CoverImage = book.CoverImage,
                Authors = book.Authors.Select(a => new AuthorSummaryDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                }).ToList(),
                Categories = book.Categories.Select(c => new CategorySummaryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
        }
        public async Task<BookForReadOnlyDTO?> GetBookByISBN(string ISBN)
        {
            var book = await _bookRepository.GetByISBNAsync(ISBN);
            if (book == null)
                return null;
            return new BookForReadOnlyDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CoverImage = book.CoverImage,
                Authors = book.Authors.Select(a => new AuthorSummaryDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                }).ToList()
            };
        }
        public async Task AddBook(CreateBookDTO bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Isbn = bookDto.ISBN,
                Description = bookDto.Description,
                PublishYear = bookDto.PublishYear,
                TotalCopies = bookDto.CopiesCount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CoverImage = bookDto.CoverImage,
                Authors = new List<Author>(),
                Categories = new List<Category>()
            };
            await _bookRepository.AddAsync(book);
        }
        public async Task UpdateBook(int id, UpdateBookDTO dto)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                throw new Exception("Book Not Found");

            book.Title = dto.Title;
            book.Isbn = dto.ISBN;
            book.Description = dto.Description;
            book.PublishYear = dto.PublishYear;
            book.TotalCopies = dto.TotalCopies;
            book.AvailableCopies = dto.AvailableCopies;
            book.IsAvailable = dto.IsAvailable;
            book.UpdatedAt = DateTime.UtcNow;
            book.CoverImage = dto.CoverImage;
            book.Authors = new List<Author>();
            book.Categories = new List<Category>();
            await _bookRepository.UpdateAsync(book);
        }
        public async Task DeleteBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                throw new Exception("Book Not Found");

            await _bookRepository.DeleteAsync(book);


        }
    }
}
