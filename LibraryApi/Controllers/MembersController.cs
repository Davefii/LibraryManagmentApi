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
        [HttpGet("ListMembers", Name = "ListMembers")]
        public async Task<IActionResult> GetAllMembers()
        {
            return Ok(await _memberService.GetAllMembers());
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("Me", Name = "GetMyMember")]
        public async Task<IActionResult> GetMemberOnlyMe()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var member = await _memberService.GetByUserId(currentUserId);

            if (member == null )
                return NotFound();
            else
                return Ok(member);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("GetMemberBy{ID}", Name = "GetMemberByID")]
        public async Task<IActionResult> GetMemberByID(int ID)
        {
            var member = await _memberService.GetMemberById(ID);

            if (member == null)
                return NotFound();
            else
                return Ok(member);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpPost("AddMemeber", Name = "AddMemeber")]
        public async Task<IActionResult> AddMember(CreateMemberDTO dto)
        {
            var currentUserId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier)!
                        .Value);

            await _memberService.AddMember(
                currentUserId,
                dto);

            return Ok();
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpPut("UpdateMyInfomationMember", Name = "UpdateMyInformationMember")]
        public async Task<IActionResult> UpdateMemberForSelf(UpdateMemberDTO dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var member = await _memberService.GetByUserId(currentUserId);

            if (member == null)
                return NotFound();


            /*bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!isAdmin &&
                member.UserId != currentUserId)
            {
                return Forbid();
            }*/

            await _memberService.UpdateMember(member.Id, dto);

            return NoContent();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPut("UpdateMember{ID}", Name = "UpdateMemberByID")]
        public async Task<IActionResult> UpdateMember(int ID,UpdateMemberDTO dto)
        {
            var member = await _memberService.GetMemberById(ID);

            if (member == null)
                return NotFound();

            await _memberService.UpdateMember(ID, dto);

            return NoContent();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpDelete("DeleteMemberBy{id}", Name = "DeleteMemberByID")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            await _memberService.DeleteMember(id);

            return Ok("Member Deleted Successfully");
        }
        //[HttpPut("UpdateUserProfileByUser{id}", Name = "UpdateUserProfileByUserID")]
        //public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDTO dto)
        //{
        //    var currentUserId =
        //        int.Parse(
        //            User.FindFirst(ClaimTypes.NameIdentifier)!
        //                .Value);
        //    await _userProfileService
        //        .UpdateProfile(
        //            currentUserId,
        //            dto);
        //    return NoContent();
        //}
    }
}
