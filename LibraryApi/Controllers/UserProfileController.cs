using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : Controller
    {
        private readonly UserProfileService _profileService;

        public UserProfileController(UserProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/userprofiles
        [HttpGet("AllProfileUser", Name = "AllProfileUser")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _profileService.GetAllProfiles();

            return Ok(profiles);
        }

        // GET: api/userprofiles/5
        [HttpGet("GetProfileBy{id}", Name = "GetProfileById")]
        public async Task<IActionResult> GetProfileById(int id)
        {
            var profile = await _profileService.GetProfileById(id);

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        // POST: api/userprofiles
        [HttpPost("CreateProfileUser", Name = "CreateProfileUser")]
        public async Task<IActionResult> AddProfile(
            CreateUserProfileDTO dto)
        {
            await _profileService.AddProfile(dto);

            return Ok("Profile Created Successfully");
        }

        // PUT: api/userprofiles/5
        [HttpPut("UpdateProfile{id}", Name = "UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(
            int id,
            UpdateUserProfileDTO dto)
        {
            await _profileService.UpdateProfile(id, dto);

            return Ok("Profile Updated Successfully");
        }

        // DELETE: api/userprofiles/5
        [HttpDelete("DeleteProfile{id}", Name = "DeleteProfile")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            await _profileService.DeleteProfile(id);

            return Ok("Profile Deleted Successfully");
        }
    }
}
