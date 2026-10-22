using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDTO>> GetAllCategories();
        Task<CategoryResponseDTO?> GetCategoryById(int id);
        Task<bool> isExistCategory(int id);
        Task AddCategory(CreateCategoryDTO dto);
        Task UpdateCategory(int id, UpdateCategoryDTO dto);
        Task DeleteCategory(int id);
    }
}
