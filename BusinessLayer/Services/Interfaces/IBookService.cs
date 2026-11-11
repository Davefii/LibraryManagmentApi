using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IBookService
    {
        Task<List<BookResponseDTO>> GetAllBooks();
        Task<List<BookResponseDTO>> GetAllByCategory(int categoryId);
        Task<List<BookResponseDTO>> GetAllByAuthor(int authorId);
        Task<List<BookResponseDTO>> GetListBooksByTitle(string title);
        Task<List<BookResponseDTO>> GetListBooksByISBN(string isbn);
        Task<BookResponseDTO?> GetBookById(int id);
        Task<BookResponseDTO?> GetBookByTitle(string title);
        Task<BookResponseDTO?> GetBookByISBN(string isbn);
        Task<int> AddBook(CreateBookDTO dto);
        Task UpdateBook(int id, UpdateBookDTO dto);
        Task DeleteBook(int id);
        Task<List<BookResponseDTO>> GetPapularBooks();
        Task<int> GetTotalBookByAuthor(int authorId);
        Task<int> GetTotalBookByCategory(int categoryId);
    }
}
