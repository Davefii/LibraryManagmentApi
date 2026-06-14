using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]

    public class BorrowingsController : Controller
    {
        private readonly BorrowingService _borrowingService;

        public BorrowingsController(
            BorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("ListBorrowings", Name = "ListBorrowings")]
        public async Task<IActionResult> GetAllBorrowings()
        {
            return Ok(
                await _borrowingService.GetAllBorrowings());
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
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
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpPost("AddBorrowing", Name = "AddBorrowing")]
        public async Task<IActionResult> AddBorrowing(
            CreateBorrowingDTO dto)
        {
            await _borrowingService.AddBorrowing(dto);

            return Ok("Borrowing Created");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPut("UpdateBorrwing{id}", Name = "UpdateBorrwing")]
        public async Task<IActionResult> UpdateBorrowing(
            int id,
            UpdateBorrowingDTO dto)
        {
            await _borrowingService
                .UpdateBorrowing(id, dto);

            return Ok("Borrowing Updated");
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpPost("returnBookBy{id}", Name = "ReturnBook")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            await _borrowingService.ReturnBook(id);

            return Ok("Book Returned Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpDelete("DeleteBorrowings{id}", Name = "DeleteBorrowing")]
        public async Task<IActionResult> DeleteBorrowing(
            int id)
        {
            await _borrowingService.DeleteBorrowing(id);

            return Ok("Borrowing Deleted");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("ListOverDueBooks", Name = "ListOverDueBooks")]
        public async Task<IActionResult> GetOverdueBorrowings()
        {
            var borrowings =
                await _borrowingService.GetOverdueBorrowings();

            return Ok(borrowings);
        }
    }
}
