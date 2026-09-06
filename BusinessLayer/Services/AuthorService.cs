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
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;

        public AuthorService(AuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<AuthorResponseDTO>> GetAllAuthors()
        {
            var Authors = await _authorRepository.GetAllAsync();
            return Authors.Select(authors => new AuthorResponseDTO
            {
                Id = authors.Id,
                FirstName = authors.FirstName,
                LastName = authors.LastName,
                Biography = authors.Biography,
                Nationality = authors.Nationality,
                BirthDate = authors.BirthDate,
                ImageAuthor = authors.ImageAuthor,
                TotalBooks = authors.Books.Count,
                Books = authors.Books.Select(book => new Book
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    Description = book.Description,
                    PublishYear = book.PublishYear,
                    CoverImage = book.CoverImage,
                }).ToList(),
            }).ToList();
        }

        public async Task<Author?> GetAuthorById(int id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }
        
        public async Task<bool> isExistAuthor(int id)
        {
            return await _authorRepository.ExistsAuthorAsync(id);
        }

        public async Task AddAuthor(CreateAuthorDTO dto)
        {
            var author = new Author
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Biography = dto.Biography,
                Nationality = dto.Nationality,
                BirthDate = dto.BirthDate,
                ImageAuthor = dto.ImageAuthor
            };

            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAuthor(int id, UpdateAuthorDTO dto)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
                throw new Exception("Author Not Found");

            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            author.Biography = dto.Biography;
            author.Nationality = dto.Nationality;
            author.BirthDate = dto.BirthDate;
            author.ImageAuthor = dto.ImageAuthor;

            await _authorRepository.UpdateAsync(author);
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
                throw new Exception("Author Not Found");

            await _authorRepository.DeleteAsync(author);
        }
    }
}
