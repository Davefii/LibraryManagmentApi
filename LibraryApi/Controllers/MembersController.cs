using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : Controller
    {
        private readonly MemberService _memberService;

        public MembersController(MemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("ListMemebers", Name = "ListMemebers")]
        public async Task<IActionResult> GetAllMembers()
        {
            return Ok(await _memberService.GetAllMembers());
        }

        [HttpGet("GetMemberBy{id}", Name = "GetMemberByID")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member =
                await _memberService.GetMemberById(id);

            if (member == null)
                return NotFound();

            return Ok(member);
        }

        [HttpPost("AddMemeber", Name = "AddMemeber")]
        public async Task<IActionResult> AddMember(
            CreateMemberDTO dto)
        {
            await _memberService.AddMember(dto);

            return Ok("Member Created Successfully");
        }

        [HttpPut("UpdateMemeberBy{id}", Name = "UpdateMemeberByID")]
        public async Task<IActionResult> UpdateMember(
            int id,
            UpdateMemberDTO dto)
        {
            await _memberService.UpdateMember(id, dto);

            return Ok("Member Updated Successfully");
        }

        [HttpDelete("DeleteMemeberBy{id}", Name = "DeleteMemeberByID")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            await _memberService.DeleteMember(id);

            return Ok("Member Deleted Successfully");
        }
    }
}
