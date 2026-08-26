using BCrypt.Net;
using BusinessLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponseDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }).ToList();
        }

        public async Task<UserResponseDTO?> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            return new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task AddUser(CreateUserDTO dto)
        {
            var existingUser =
                await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
                throw new Exception("Email already exists");

            if (dto.Role != Roles.Admin &&
                dto.Role != Roles.Member)
            {
                throw new Exception("Invalid Role");
            }

            var user = new User
            {
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUser(
            int id,
            UpdateUserDTO dto)
        {
            var user =
                await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User Not Found");

            user.Email = dto.Email;
            if (dto.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }
            user.IsActive = dto.IsActive;
            user.Role = dto.Role;
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUser(int id)
        {
            var user =
                await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User Not Found");

            await _userRepository.DeleteAsync(user);
        }
    
    }
}
