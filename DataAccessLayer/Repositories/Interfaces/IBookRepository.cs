using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetAllByCategoryAsync(int categoryId);
        Task<List<Book>> GetAllByAuthorAsync(int authorId);
        Task<Book?> GetByIdAsync(int id);
        Task<List<Book>> GetBookListByTitleAsync(string title);
        Task<List<Book>> GetBookListByISBNAsync(string isbn);
        Task<Book?> GetByTitleAsync(string title);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<int> AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<int> TotalBooks();
        Task<int> TotalBookByCategorie(int categoryId);
        Task<int> TotalBookByAuthor(int authorId);
        Task<int> GetUnavailableBooksCountAsync();
        Task<List<Book>> GetPopularBooks();
    }
}
