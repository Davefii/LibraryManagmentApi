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
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponseDTO>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(category => new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentId = category.ParentId
            }).ToList();
        }

        public async Task<CategoryResponseDTO?> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentId = category.ParentId
            };
        }

        public async Task<bool> isExistCategory(int id)
        {
            return await _categoryRepository.ExistsAsync(id);
        }


        public async Task AddCategory(CreateCategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                ParentId = dto.ParentId
            };

            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategory(int id, UpdateCategoryDTO dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new Exception("Category Not Found");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ParentId = dto.ParentId;

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new Exception("Category Not Found");

            await _categoryRepository.DeleteAsync(category);
        }
    }

}
