using BusinessLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IAuthorService
    {
        Task<List<AuthorResponseDTO>> GetAllAuthors();
        Task<Author?> GetAuthorById(int id);
        Task<bool> isExistAuthor(int id);
        Task AddAuthor(CreateAuthorDTO dto);
        Task UpdateAuthor(int id, UpdateAuthorDTO dto);
        Task DeleteAuthor(int id);
    }
}
