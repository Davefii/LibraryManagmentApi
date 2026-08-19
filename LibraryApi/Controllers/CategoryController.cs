using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [AllowAnonymous]
        [HttpGet("GetAllCategorysForAnyone", Name = "GetAllCategorysForAnyone")]
        public async Task<IActionResult> GetAllCategoriesForAnyone()
        {
            var categories = await _categoryService.GetAllCategories();

            return Ok(categories);
        }
        // GET: api/categories
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetAllCategorys", Name = "GetAllCategorys")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();

            return Ok(categories);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        // GET: api/categories/5
        [HttpGet("GetCategoryBy{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        // POST: api/categories
        [HttpPost("CreateCategory", Name = "CreateCategory")]
        public async Task<IActionResult> AddCategory(CreateCategoryDTO dto)
        {
            await _categoryService.AddCategory(dto);

            return Ok("Category Added Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        // PUT: api/categories/5
        [HttpPut("UpdateCategoryBy{id}", Name = "UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            UpdateCategoryDTO dto)
        {
            await _categoryService.UpdateCategory(id, dto);

            return Ok("Category Updated Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        // DELETE: api/categories/5
        [HttpDelete("DeleteCategoryBy{id}", Name = "DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);

            return Ok("Category Deleted Successfully");
        }
    }
}
