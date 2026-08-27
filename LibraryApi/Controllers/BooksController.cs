
using Azure.Core;
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using LibraryApi.Requests;
using LibraryApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class BooksController : Controller
    {
        private readonly BookService _bookservice;
        private readonly ImageService _imageService;
        private readonly AuthorService _author;
        private readonly CategoryService _category;
        public BooksController(BookService bookService, ImageService imageService, AuthorService author, CategoryService category)
        {
            _bookservice = bookService;
            _imageService = imageService;
            _author = author;
            _category = category;
        }
        [AllowAnonymous]
        [HttpGet("ListBooksForAnyone", Name = "GetallBooksForAnyone")]
        public async Task<IActionResult> GetAllBooksForAnyone()
        {
            var books = await _bookservice.GetAllBooks();

            return Ok(books);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("ListBooks", Name = "GetallBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookservice.GetAllBooks();

            return Ok(books);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetBookByID{id}", Name = "GetBookByID")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookservice.GetBookById(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetBookByName/{title}", Name = "GetBookByName")]
        public async Task<IActionResult> GetBookByName(string title)
        {
            var book = await _bookservice.GetBookByTitle(title);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetBookByISBN/{isbn}", Name = "GetBookByISBN")]
        public async Task<IActionResult> GetBookByISBN([FromRoute] string isbn)
        {
            var book = await _bookservice.GetBookByISBN(isbn);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPost("AddBook", Name = "AddBook")]
        public async Task<IActionResult> AddBook([FromForm] CreateBookRequest request)
        {
            string? imagePath = null;

            if (request.CoverImage != null)
            {
                imagePath =
                    await _imageService.SaveImageAsync(
                        request.CoverImage,
                        ImageFolders.Books);
            }


            var bookdto = new CreateBookDTO
            {
                Title = request.Title.Trim(),
                ISBN = request.ISBN.Trim(),
                Description = request.Description.Trim(),
                PublishYear = request.PublishYear,
                TotalCopies = request.TotalCopies,
                AvailableCopies = request.AvailableCopies,
                IsAvailable = request.IsAvailable,
                CoverImage = imagePath,
                AuthorID = request.AuthorID,
                CategoryID = request.CategoryID,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt
            };
            await _bookservice.AddBook(bookdto);

            return Ok();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPut("UpdateBookBy{Id}", Name = "UpdateBookByid")]
        public async Task<IActionResult> UpdateBook([FromRoute] int Id, [FromForm] UpdateBookRequest request)
        {
            var currentBook = await _bookservice.GetBookById(Id);

            if (currentBook == null)
                return NotFound();

            string? imagePath = currentBook.CoverImage;

            if (request.CoverImage != null)
            {

                var newImage =
                    await _imageService.SaveImageAsync(
                        request.CoverImage,
                        ImageFolders.Books);

                _imageService.DeleteImage(currentBook.CoverImage);
                imagePath = newImage;
            }

            var dto = new UpdateBookDTO
            {
                Title = request.Title.Trim(),
                ISBN = request.ISBN.Trim(),
                Description = request.Description.Trim(),
                PublishYear = request.PublishYear,
                TotalCopies = request.TotalCopies,
                AvailableCopies = request.AvailableCopies,
                IsAvailable = request.IsAvailable,
                CoverImage = imagePath,
                AuthorID = request.AuthorID,
                CategoryID = request.CategoryID,
                
                UpdatedAt = DateTime.UtcNow
            };
            await _bookservice.UpdateBook(Id, dto);

            return Ok("Book Updated Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpDelete("DeleteBookBy{ID}", Name = "DeleteBookByID")]
        public async Task<IActionResult> DeleteBook(int ID)
        {

            var book = await _bookservice.GetBookById(ID);

            if (book == null)
                return NotFound();

            _imageService.DeleteImage(book.CoverImage);

            await _bookservice.DeleteBook(book.Id);

            

            return Ok("Book Deleted Successfully");
        }
    }
}
