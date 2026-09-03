using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
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
        [HttpGet("GetBorrowingBy/{id}", Name = "GetBorrowingById")]
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

        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpPost("AddBorrowing", Name = "AddBorrowing")]
        public async Task<IActionResult> AddBorrowing(
            CreateBorrowingDTO dto)
        {
            await _borrowingService.AddBorrowing(dto);

            return Ok("Borrow Book Successfulley");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPut("UpdateBorrowing/{id}", Name = "UpdateBorrowing")]
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
        [HttpPost("returnBookBy/{id}", Name = "ReturnBook")]
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
        [HttpDelete("DeleteBorrowings/{id}", Name = "DeleteBorrowing")]
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

        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("RecentBorrowingsForMember", Name = "RecentBorrowingsForMemberCurrent")]
        [HttpGet("RecentBorrowingsForMember/{memberId}", Name = "RecentBorrowingsForMember")]
        public async Task<IActionResult> GetRecentBorrowingsForMember(int? memberId = null)
        {
            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!memberId.HasValue)
            {
                memberId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);

                if (!memberId.HasValue)
                    return BadRequest(new { message = "User does not have a member profile" });
            }

            if (!isAdmin)
            {
                var memberUserId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);
                if (memberId != memberUserId)
                {
                    return Forbid();
                }
            }

            var recentBorrowings = 
                await _borrowingService.RecentborrowingsForSelfMemberAsync(memberId.Value);

            return Ok(recentBorrowings);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("TotalBorrowingsForMember", Name = "TotalBorrowingsForMemberCurrent")]
        [HttpGet("TotalBorrowingsForMember/{memberId}", Name = "TotalBorrowingsForMember")]
        public async Task<IActionResult> GetTotalBorrowingsForMember(int? memberId = null)
        {
            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!memberId.HasValue)
            {
                memberId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);

                if (!memberId.HasValue)
                    return BadRequest(new { message = "User does not have a member profile" });
            }

            if (!isAdmin)
            {
                var memberUserId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);
                if (memberId != memberUserId)
                {
                    return Forbid();
                }
            }

            var totalBorrowings = 
                await _borrowingService.TotalBorrowingsForSelfMemberAsync(memberId.Value);

            return Ok(new { totalBorrowings });
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("ActiveBorrowingsForMember", Name = "ActiveBorrowingsForMemberCurrent")]
        [HttpGet("ActiveBorrowingsForMember/{memberId}", Name = "ActiveBorrowingsForMember")]
        public async Task<IActionResult> GetActiveBorrowingsForMember(int? memberId = null)
        {
            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!memberId.HasValue)
            {
                memberId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);

                if (!memberId.HasValue)
                    return BadRequest(new { message = "User does not have a member profile" });
            }

            if (!isAdmin)
            {
                var memberUserId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);
                if (memberId != memberUserId)
                {
                    return Forbid();
                }
            }

            var activeBorrowings = 
                await _borrowingService.TotalActiveBorrowingsForSelfMemberAsync(memberId.Value);

            return Ok(new { activeBorrowings });
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("ReturnedBorrowingsForMember", Name = "ReturnedBorrowingsForMemberCurrent")]
        [HttpGet("ReturnedBorrowingsForMember/{memberId}", Name = "ReturnedBorrowingsForMember")]
        public async Task<IActionResult> GetReturnedBorrowingsForMember(int? memberId = null)
        {
            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!memberId.HasValue)
            {
                memberId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);

                if (!memberId.HasValue)
                    return BadRequest(new { message = "User does not have a member profile" });
            }

            if (!isAdmin)
            {
                var memberUserId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);
                if (memberId != memberUserId)
                {
                    return Forbid();
                }
            }

            var returnedBorrowings = 
                await _borrowingService.TotalReturnBorrowingsForSelfMemberAsync(memberId.Value);

            return Ok(new { returnedBorrowings });
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("OverdueBorrowingsForMember", Name = "OverdueBorrowingsForMemberCurrent")]
        [HttpGet("OverdueBorrowingsForMember/{memberId}", Name = "OverdueBorrowingsForMember")]
        public async Task<IActionResult> GetOverdueBorrowingsForMember(int? memberId = null)
        {
            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!memberId.HasValue)
            {
                memberId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);

                if (!memberId.HasValue)
                    return BadRequest(new { message = "User does not have a member profile" });
            }

            if (!isAdmin)
            {
                var memberUserId = await _borrowingService.GetMemberIdByUserIdAsync(currentUserId);
                if (memberId != memberUserId)
                {
                    return Forbid();
                }
            }

            var overdueBorrowings = 
                await _borrowingService.TotalOverdueBorrowingsForSelfMemberAsync(memberId.Value);

            return Ok(new { overdueBorrowings });
        }
    }
}
