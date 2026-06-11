using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    public class BorrowingsController : Controller
    {
        private readonly BorrowingService _borrowingService;

        public BorrowingsController(
            BorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        [HttpGet("ListBorrowings", Name = "ListBorrowings")]
        public async Task<IActionResult> GetAllBorrowings()
        {
            return Ok(
                await _borrowingService.GetAllBorrowings());
        }

        [HttpGet("GetBorrowingBy{id}", Name = "GetBorrowingById")]
        public async Task<IActionResult> GetBorrowingById(
            int id)
        {
            var borrowing =
                await _borrowingService
                    .GetBorrowingById(id);

            if (borrowing == null)
                return NotFound();

            return Ok(borrowing);
        }

        [HttpPost("AddBorrowing", Name = "AddBorrowing")]
        public async Task<IActionResult> AddBorrowing(
            CreateBorrowingDTO dto)
        {
            await _borrowingService.AddBorrowing(dto);

            return Ok("Borrowing Created");
        }

        [HttpPut("UpdateBorrwing{id}", Name = "UpdateBorrwing")]
        public async Task<IActionResult> UpdateBorrowing(
            int id,
            UpdateBorrowingDTO dto)
        {
            await _borrowingService
                .UpdateBorrowing(id, dto);

            return Ok("Borrowing Updated");
        }
        [HttpPost("returnBookBy{id}", Name = "ReturnBook")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            await _borrowingService.ReturnBook(id);

            return Ok("Book Returned Successfully");
        }
        [HttpDelete("DeleteBorrowings{id}", Name = "DeleteBorrowing")]
        public async Task<IActionResult> DeleteBorrowing(
            int id)
        {
            await _borrowingService.DeleteBorrowing(id);

            return Ok("Borrowing Deleted");
        }
        [HttpGet("ListOverDueBooks", Name = "ListOverDueBooks")]
        public async Task<IActionResult> GetOverdueBorrowings()
        {
            var borrowings =
                await _borrowingService.GetOverdueBorrowings();

            return Ok(borrowings);
        }
    }
}
