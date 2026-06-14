using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("ListAuthors", Name = "ListAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await _authorService.GetAllAuthors());
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetAuthorBy{id}", Name = "GetAuthorByID")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorById(id);

            if (author == null)
                return NotFound();

            return Ok(author);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPost("AddAuthor", Name = "AddAuthor")]
        public async Task<IActionResult> AddAuthor(CreateAuthorDTO dto)
        {
            await _authorService.AddAuthor(dto);

            return Ok();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPut("UpdateAuthor{id}", Name = "UpdateAuthorbyID")]
        public async Task<IActionResult> UpdateAuthor(
            int id,
            UpdateAuthorDTO dto)
        {
            await _authorService.UpdateAuthor(id, dto);

            return Ok();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpDelete("DeleteAuthor{id}", Name = "DeleteAuthorbyID")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAuthor(id);

            return Ok();
        }
    }
}
