using Azure.Core;
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.RateLimiting;
namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly AuditService _auditService;
        public AuthController(UserRepository userRepository, RefreshTokenService refreshTokenService, ILogger<AuthController> logger, AuditService auditService)
        {
            _userRepository = userRepository;
            _refreshTokenService = refreshTokenService;
            _logger = logger;
            _auditService = auditService;
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {

            var user =
                await _userRepository
                    .GetByEmailAsync(request.Email);

            if (user == null)
            {
                //_logger.LogWarning("Failed login attempt for {Email}",request.Email);
                await _auditService.LogAsync(null,"FAILED_LOGIN","USER",$"Unknown email: {request.Email}");
                return Unauthorized("Invalid Credentials");
            }


            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.Password);

            if (!isValidPassword)
            {
                //_logger.LogWarning("Invalid password for {Email}",request.Email);
                await _auditService.LogAsync(user.Id,"FAILED_LOGIN","USER",$"Invalid password for {user.Email}");
                return Unauthorized("Invalid Credentials");
            }

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
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity =
            new RefreshToken
            {
                UserId = user.Id,

                Token =
                    BCrypt.Net.BCrypt.HashPassword(
                        refreshToken),

                ExpiresAt =
                    DateTime.UtcNow.AddDays(1),

                CreatedAt =
                    DateTime.UtcNow
            };

             await _refreshTokenService.AddAsync(refreshTokenEntity);
            //_logger.LogInformation("User {Email} logged in successfully",user.Email);
            await _auditService.LogAsync(user.Id,"LOGIN","USER",$"User {user.Email} logged in successfully");
            return Ok(new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Refresh(RefreshRequestDTO request)
        {
            var user =
                await _refreshTokenService
                    .ValidateRefreshToken(
                        request.Email,
                        request.RefreshToken);

            if (user == null)
                return Unauthorized(
                    "Invalid Refresh Token");

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

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSecret!));

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var jwt =
                new JwtSecurityToken(
                    issuer: "LibraryApi",
                    audience: "LibraryUsers",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

            var accessToken =
                new JwtSecurityTokenHandler()
                    .WriteToken(jwt);

            // Token Rotation
            await _refreshTokenService
                .RevokeToken(
                    request.Email,
                    request.RefreshToken);

            var newRefreshToken =
                GenerateRefreshToken();

            var refreshTokenEntity =
                new RefreshToken
                {
                    UserId = user.Id,

                    Token =
                        BCrypt.Net.BCrypt.HashPassword(
                            newRefreshToken),

                    ExpiresAt =
                        DateTime.UtcNow.AddDays(7),

                    CreatedAt =
                        DateTime.UtcNow
                };

            await _refreshTokenService
                .AddAsync(refreshTokenEntity);

            return Ok(
                new TokenResponseDTO
                {
                    AccessToken =
                        accessToken,

                    RefreshToken =
                        newRefreshToken
                });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshRequestDTO request)
        {
            await _refreshTokenService
                .RevokeToken(
                    request.Email,
                    request.RefreshToken);

            return Ok(
                "Logged Out Successfully");
        }
    }
}
