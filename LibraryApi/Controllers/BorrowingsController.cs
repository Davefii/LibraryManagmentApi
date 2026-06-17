using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Authorize]

    public class BorrowingsController : Controller
    {
        private readonly BorrowingService _borrowingService;
        private readonly MemberService _memberService;
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

            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!isAdmin &&
                borrowing.UserId != currentUserId)
            {
                return Forbid();
            }

            return Ok(borrowing);
        }

    //    [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
    //    [HttpGet("{memberId}/history")]
    //    public async Task<IActionResult>
    //GetBorrowingHistory(int memberId)
    //    {
    //        var member =
    //            await _memberService
    //                .GetMemberById(memberId);

    //        if (member == null)
    //            return NotFound();

    //        var currentUserId =
    //            int.Parse(
    //                User.FindFirst(ClaimTypes.NameIdentifier)!
    //                    .Value);

    //        bool isAdmin =
    //            User.IsInRole(Roles.Admin);

    //        if (!isAdmin &&
    //            member.UserId != currentUserId)
    //        {
    //            return Forbid();
    //        }

    //        var history =
    //            await _borrowingService
    //                .GetMemberBorrowings(memberId);

    //        return Ok(history);
    //    }

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
            var borrowing =
                    await _borrowingService
                        .GetBorrowingById(id);

            if (borrowing == null)
                return NotFound();

            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!isAdmin &&
                borrowing.UserId != currentUserId)
            {
                return Forbid();
            }

            await _borrowingService
                .UpdateBorrowing(id, dto);

            return NoContent();
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpPost("returnBookBy{id}", Name = "ReturnBook")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var borrowing =
                    await _borrowingService
                        .GetBorrowingById(id);

            if (borrowing == null)
                return NotFound();

            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!isAdmin &&
                borrowing.UserId != currentUserId)
            {
                return Forbid();
            }

            await _borrowingService.ReturnBook(id);

            return NoContent();
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
