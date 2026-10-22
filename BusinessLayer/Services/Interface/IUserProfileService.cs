using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IUserProfileService
    {
        Task<List<UserProfileResponseDTO>> GetAllProfiles();
        Task<UserProfileResponseDTO?> GetProfileById(int id);
        Task<UserProfileResponseDTO?> GetByUserId(int userId);
        Task AddProfile(CreateUserProfileDTO dto);
        Task UpdateProfile(int id, UpdateUserProfileDTO dto);
        Task DeleteProfile(int id);
    }
}
