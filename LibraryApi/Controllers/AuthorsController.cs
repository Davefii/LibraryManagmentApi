using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("ListAuthors", Name = "ListAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await _authorService.GetAllAuthors());
        }

        [HttpGet("GetAuthorBy{id}", Name = "GetAuthorByID")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorById(id);

            if (author == null)
                return NotFound();

            return Ok(author);
        }

        [HttpPost("AddAuthor", Name = "AddAuthor")]
        public async Task<IActionResult> AddAuthor(CreateAuthorDTO dto)
        {
            await _authorService.AddAuthor(dto);

            return Ok();
        }

        [HttpPut("UpdateAuthor{id}", Name = "UpdateAuthorbyID")]
        public async Task<IActionResult> UpdateAuthor(
            int id,
            UpdateAuthorDTO dto)
        {
            await _authorService.UpdateAuthor(id, dto);

            return Ok();
        }

        [HttpDelete("DeleteAuthor{id}", Name = "DeleteAuthorbyID")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAuthor(id);

            return Ok();
        }
    }
}
