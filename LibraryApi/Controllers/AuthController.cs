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
            Response.Cookies.Append(
               "access_token",
               accessToken,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,
                   SameSite = SameSiteMode.None,
                   Expires = DateTimeOffset.UtcNow.AddMinutes(30)
               });

                       Response.Cookies.Append(
                           "refresh_token",
                           refreshToken,
                           new CookieOptions
                           {
                               HttpOnly = true,
                               Secure = true,
                               SameSite = SameSiteMode.None,
                               Expires = DateTimeOffset.UtcNow.AddDays(1)
                           });
            await _refreshTokenService.AddAsync(refreshTokenEntity);
            //_logger.LogInformation("User {Email} logged in successfully",user.Email);
            await _auditService.LogAsync(user.Id,"LOGIN","USER",$"User {user.Email} logged in successfully");
            return Ok(new
            {
                message = "Login successful"
            });
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(
                    "Refresh token not found.");


            var user =
                await _refreshTokenService
                    .ValidateRefreshToken(refreshToken);

            if (user == null || !user.IsActive)
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
            if (string.IsNullOrEmpty(jwtSecret))
                throw new Exception(
                    "JWT Secret Not Found");

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
                .RevokeToken(refreshToken);

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
                        DateTime.UtcNow.AddDays(1),

                    CreatedAt =
                        DateTime.UtcNow
                };

            await _refreshTokenService
                .AddAsync(refreshTokenEntity);

            // New access token Cookie
            Response.Cookies.Append(
                "access_token",
                accessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires =
                        DateTimeOffset.UtcNow.AddMinutes(30)
                });


            // New refresh token Cookie
            Response.Cookies.Append(
                "refresh_token",
                newRefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires =
                        DateTimeOffset.UtcNow.AddDays(7)
                });


            return Ok(new
            {
                message =
                    "Token refreshed successfully"
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken =
                Request.Cookies["refresh_token"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _refreshTokenService
                    .RevokeToken(refreshToken);
            }

            Response.Cookies.Delete(
                "access_token",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

            Response.Cookies.Delete(
                "refresh_token",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

            return Ok(new
            {
                message = "Logged out successfully"
            });
        }
    }
}
