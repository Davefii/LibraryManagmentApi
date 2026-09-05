using BusinessLayer.DTOs;
using BusinessLayer.Services;
using LibraryApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly MemberService _memberService;

        public UsersController(UserService userService,MemberService memberService)
        {
            _userService = userService;
            _memberService = memberService;
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        // GET: api/users
        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();

            return Ok(users);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        // GET: api/users/5
        [HttpGet("GetUserBy{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            bool isAdmin = User.IsInRole(Roles.Admin);

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }

            var user =
                await _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [AllowAnonymous]
        // POST: api/users
        [HttpPost("CreateUserOnlyMember", Name = "CreateUser")]
        public async Task<IActionResult> CreateUserForMember([FromBody] RegisterRequestDTO dto)
        {
            var createDto = new CreateUserDTO
            {
                Email = dto.Email,
                Password = dto.Password,
                Role = Roles.Member,
                IsActive = true
            };
            int userID = await _userService.AddUser(createDto);

            var createMemberdto = new CreateMemberDTO
            {
                Name = dto.FullName,
                Phone = dto.PhoneNumber,
                Address = dto.Address,
                MembershipExpiryDate = DateTime.UtcNow.AddYears(5)
            };
            await _memberService.AddMember(userID,createMemberdto);
            return Ok("User Created Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        // POST: api/users
        [HttpPost("AddUser",Name = "AddUser")]
        public async Task<IActionResult> AddUser(
            [FromBody] CreateUserDTO dto)
        {

            await _userService.AddUser(dto);

            return Ok("User Created Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        // PUT: api/users/5
        [HttpPut("UpdateUserBy{id}" ,Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserDTO dto)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUserId =
                int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!
                        .Value);

            bool isAdmin =
                User.IsInRole(Roles.Admin);

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }

            await _userService.UpdateUser(id, dto);

            return NoContent();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        // DELETE: api/users/5
        [HttpDelete("DeleteUserBy{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);

            return Ok("User Deleted Successfully");
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        [HttpGet("GetCurrentUser", Name = "GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userService.GetUserById(currentUserId);
            var Curentuser = new CurrentUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };
            return Ok(Curentuser);
        }
    }
}
