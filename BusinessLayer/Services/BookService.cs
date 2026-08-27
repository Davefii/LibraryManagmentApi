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
        private readonly AuthorRepository _authorRepository;
        private readonly CategoryRepository _categoryRepository;

        public BookService(BookRepository bookRepository, AuthorRepository authorRepository, CategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<List<BookResponseDTO>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllAsync();

            return books.Select(book => new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CopiesCount= book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
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
        public async Task<BookResponseDTO?> GetBookById(int id)
        {
           
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                return null;
            else
            return new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CopiesCount = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
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
        public async Task<BookResponseDTO?> GetBookByTitle(string title)
        {
            var book = await _bookRepository.GetByTitleAsync(title);
            if (book == null)
                return null;
            else
            return new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CopiesCount = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
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
        public async Task<BookResponseDTO?> GetBookByISBN(string ISBN)
        {
            var book = await _bookRepository.GetByISBNAsync(ISBN);
            if (book == null)
                return null;
            else
            return new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                Description = book.Description,
                PublishYear = book.PublishYear,
                CopiesCount = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
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
        public async Task AddBook(CreateBookDTO bookDto)
        {
            var author = await _authorRepository.GetByIdAsync(bookDto.AuthorID);
            var category = await _categoryRepository.GetByIdAsync(bookDto.CategoryID);

            if (author == null)
                throw new Exception("Author not found");

            if (category == null)
                throw new Exception("Category not found");
            var book = new Book
            {
                Title = bookDto.Title,
                Isbn = bookDto.ISBN,
                Description = bookDto.Description,
                PublishYear = bookDto.PublishYear,
                TotalCopies = bookDto.TotalCopies,
                AvailableCopies = bookDto.AvailableCopies,
                IsAvailable = bookDto.IsAvailable,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CoverImage = bookDto.CoverImage
            };

            book.Authors.Add(author);
            book.Categories.Add(category);

            await _bookRepository.AddAsync(book);
        }
        public async Task UpdateBook(int id, UpdateBookDTO dto)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                throw new Exception("Book Not Found");

            var author = await _authorRepository.GetByIdAsync(dto.AuthorID);
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryID);

            if (author == null)
                throw new Exception("Author not found");

            if (category == null)
                throw new Exception("Category not found");

            book.Title = dto.Title;
            book.Isbn = dto.ISBN;
            book.Description = dto.Description;
            book.PublishYear = dto.PublishYear;
            book.TotalCopies = dto.TotalCopies;
            book.AvailableCopies = dto.AvailableCopies;
            book.IsAvailable = dto.IsAvailable;
            book.UpdatedAt = DateTime.UtcNow;
            book.CoverImage = dto.CoverImage;
            book.Authors.Clear();
            book.Categories.Clear();

            book.Authors.Add(author);
            book.Categories.Add(category);
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
