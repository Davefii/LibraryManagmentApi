using Azure.Core;
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using LibraryApi.Requests;
using LibraryApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    // List Books where on Controllers
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly AuthorService _authorService;
        private readonly ImageService _imageService;
        public AuthorsController(AuthorService authorService, ImageService imageService)
        {
            _authorService = authorService;
            _imageService = imageService;
        }
        [AllowAnonymous]
        [HttpGet("ListAuthorsForAnyone", Name = "ListAuthorsForAnyone")]
        public async Task<IActionResult> GetAllAuthorsForAnyone()
        {
            return Ok(await _authorService.GetAllAuthors());
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
        public async Task<IActionResult> AddAuthor([FromForm] CreateAuthorRequest request)
        {
            string? imagePath = null;

            if (request.ImageAuthor != null)
            {
                imagePath = await _imageService.SaveImageAsync(
                    request.ImageAuthor,
                    ImageFolders.Authors);
            }

            var dto = new CreateAuthorDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Biography = request.Biography,
                Nationality = request.Nationality,
                BirthDate = request.BirthDate,
                // List Books where
                ImageAuthor = imagePath
            };

            await _authorService.AddAuthor(dto);

            return Ok();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPut("UpdateAuthor{id}", Name = "UpdateAuthorbyID")]
        public async Task<IActionResult> UpdateAuthor(
            [FromForm]
            int id,
            UpdateAuthorRequest request)
        {
            var currentAuthor =
        await _authorService.GetAuthorById(id);

            if (currentAuthor == null)
                return NotFound();

            string? imagePath = currentAuthor.ImageAuthor;

            if (request.ImageAuthor != null)
            {
                var newImage =
                    await _imageService.SaveImageAsync(
                        request.ImageAuthor,
                        ImageFolders.Authors);

                _imageService.DeleteImage(currentAuthor.ImageAuthor);

                imagePath = newImage;
            }

            var dto = new UpdateAuthorDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Biography = request.Biography,
                Nationality = request.Nationality,
                BirthDate = request.BirthDate,
                ImageAuthor = imagePath
            };
            await _authorService.UpdateAuthor(id, dto);

            return Ok();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpDelete("DeleteAuthor{id}", Name = "DeleteAuthorbyID")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorService.GetAuthorById(id);

            if (author == null)
                return NotFound();

            _imageService.DeleteImage(author.ImageAuthor);

            await _authorService.DeleteAuthor(id);

            return Ok();
        }
    }
}
