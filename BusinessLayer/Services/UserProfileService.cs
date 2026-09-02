using BusinessLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserProfileService
    {
        private readonly UserProfileRepository _profileRepository;
        private readonly UserRepository _userRepository;

        public UserProfileService(
            UserProfileRepository profileRepository,
            UserRepository userRepository)
        {
            _profileRepository = profileRepository;
            _userRepository = userRepository;
        }

        public async Task<List<UserProfileResponseDTO>>
            GetAllProfiles()
        {
            var profiles =
                await _profileRepository.GetAllAsync();

            return profiles.Select(profile =>
                new UserProfileResponseDTO
                {
                    Id = profile.Id,
                    UserId = profile.UserId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    PhoneNumber = profile.PhoneNumber,
                    Address = profile.Address,
                    User = new UserResponseDTO
                    {
                        Id = profile.User.Id,
                        Email = profile.User.Email,
                        Role = profile.User.Role,
                        IsActive = profile.User.IsActive,
                        CreatedAt = profile.User.CreatedAt,
                    }
                }).ToList();
        }

        public async Task<UserProfileResponseDTO?>
            GetProfileById(int id)
        {
            var profile =
                await _profileRepository.GetByIdAsync(id);

            if (profile == null)
                return null;

            return new UserProfileResponseDTO
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                User = new UserResponseDTO
                {
                    Id = profile.User.Id,
                    Email = profile.User.Email,
                    Role = profile.User.Role,
                    IsActive = profile.User.IsActive,
                    CreatedAt = profile.User.CreatedAt,
                }
            };
        }

        public async Task<UserProfileResponseDTO?>GetByUserId(int userId)
        {
            var profile =
                await _profileRepository
                    .GetByUserIdAsync(userId);

            if (profile == null)
                return null;

            return new UserProfileResponseDTO
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                User = new UserResponseDTO
                {
                    Id = profile.User.Id,
                    Email = profile.User.Email,
                    Role = profile.User.Role,
                    IsActive = profile.User.IsActive,
                    CreatedAt = profile.User.CreatedAt,
                }
            };
        }

        public async Task AddProfile(
            CreateUserProfileDTO dto)
        {
            var user =
                await _userRepository.GetByIdAsync(dto.UserId);

            if (user == null)
                throw new Exception("User Not Found");

            var existingProfile =
                await _profileRepository
                    .GetByUserIdAsync(dto.UserId);

            if (existingProfile != null)
                throw new Exception(
                    "Profile already exists");

            var profile = new UserProfile
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            await _profileRepository.AddAsync(profile);
        }

        public async Task UpdateProfile(
            int id,
            UpdateUserProfileDTO dto)
        {
            var profile =
                await _profileRepository.GetByIdAsync(id);

            if (profile == null)
                throw new Exception("Profile Not Found");

            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;
            profile.PhoneNumber = dto.PhoneNumber;
            profile.Address = dto.Address;

            await _profileRepository.UpdateAsync(profile);
        }

        public async Task DeleteProfile(int id)
        {
            var profile =
                await _profileRepository.GetByIdAsync(id);

            if (profile == null)
                throw new Exception("Profile Not Found");

            await _profileRepository.DeleteAsync(profile);
        }
    }
}
