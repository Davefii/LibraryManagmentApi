using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : Controller
    {
        private readonly MemberService _memberService;
        private readonly UserProfileService _userProfileService;

        public MembersController(MemberService memberService)
        {
            _memberService = memberService;
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("ListMemebers", Name = "ListMemebers")]
        public async Task<IActionResult> GetAllMembers()
        {
            return Ok(await _memberService.GetAllMembers());
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetMemberBy{id}", Name = "GetMemberByID")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member =
                await _memberService.GetMemberById(id);

            if (member == null)
                return NotFound();

            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!isAdmin &&
                member.UserId != currentUserId)
            {
                return Forbid();
            }

            return Ok(member);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPost("AddMemeber", Name = "AddMemeber")]
        public async Task<IActionResult> AddMember(
            CreateMemberDTO dto)
        {
            await _memberService.AddMember(dto);

            return Ok("Member Created Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpPut("UpdateMemeberBy{id}", Name = "UpdateMemeberByID")]
        public async Task<IActionResult> UpdateMember(
            int id,
            UpdateMemberDTO dto)
        {
            var member =
                    await _memberService.GetMemberById(id);

            if (member == null)
                return NotFound();

            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!isAdmin &&
                member.UserId != currentUserId)
            {
                return Forbid();
            }

            await _memberService.UpdateMember(id, dto);

            return NoContent();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpDelete("DeleteMemeberBy{id}", Name = "DeleteMemeberByID")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            await _memberService.DeleteMember(id);

            return Ok("Member Deleted Successfully");
        }
        [HttpPut("UpdateUserProfileByUser{id}", Name = "UpdateUserProfileByUserID")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDTO dto)
        {
            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            await _userProfileService
                .UpdateProfile(
                    currentUserId,
                    dto);

            return NoContent();
        }
    }
}
