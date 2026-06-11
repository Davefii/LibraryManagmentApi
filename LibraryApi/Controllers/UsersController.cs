using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();

            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("GetUserBy{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/users
        [HttpPost("AddUser",Name = "AddUser")]
        public async Task<IActionResult> AddUser(
            [FromBody] CreateUserDTO dto)
        {
            await _userService.AddUser(dto);

            return Ok("User Created Successfully");
        }

        // PUT: api/users/5
        [HttpPut("UpdateUserBy{id}" ,Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserDTO dto)
        {
            await _userService.UpdateUser(id, dto);

            return Ok("User Updated Successfully");
        }

        // DELETE: api/users/5
        [HttpDelete("DeleteUserBy{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);

            return Ok("User Deleted Successfully");
        }
    }
}
