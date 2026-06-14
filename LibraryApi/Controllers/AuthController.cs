using BusinessLayer.DTOs;
using BusinessLayer.Services;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {

            var user =
                await _userRepository
                    .GetByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized("Invalid Credentials");


            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.Password);

            if (!isValidPassword)
                return Unauthorized("Invalid Credentials");

            // Claims
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role)
            };


            var jwtSecret =
                Environment.GetEnvironmentVariable(
                    "LIBRARY_JWT_SECRET");

            if (string.IsNullOrEmpty(jwtSecret))
                throw new Exception(
                    "JWT Secret Not Found");

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSecret));

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: "LibraryApi",
                    audience: "LibraryUsers",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

            return Ok(new
            {
                token =
                    new JwtSecurityTokenHandler()
                        .WriteToken(token)
            });
        }
    }
}
