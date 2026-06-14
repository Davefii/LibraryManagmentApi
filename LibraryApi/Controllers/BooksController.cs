
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
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
        public BooksController(BookService bookService)
        {
            _bookservice = bookService;
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
        [HttpGet("GetBookByName{Titel}", Name = "GetBookByName")]
        public async Task<IActionResult> GetBookByName(string titel)
        {
            var book = await _bookservice.GetBookByTitle(titel);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetBookByISBN{ISBN}", Name = "GetBookByISBN")]
        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            var book = await _bookservice.GetBookByISBN(isbn);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPost("AddBook", Name = "AddBook")]
        public async Task<IActionResult> AddBook(CreateBookDTO book)
        {
            await _bookservice.AddBook(book);

            return Ok();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPut("UpdateBookBy{Id}", Name = "UpdateBookByid")]
        public async Task<IActionResult> UpdateBook(int Id,UpdateBookDTO dto)
        {
            await _bookservice.UpdateBook(Id, dto);

            return Ok("Book Updated Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpDelete("DeleteBookBy{ID}", Name = "DeleteBookByID")]
        public async Task<IActionResult> DeleteBook(int ID)
        {
            await _bookservice.DeleteBook(ID);

            return Ok("Book Deleted Successfully");
        }
    }
}
