using BusinessLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IUserService
    {
        Task<List<UserResponseDTO>> GetAllUsers();
        Task<UserResponseDTO?> GetUserById(int id);
        Task<UserResponseDTO?> GetUserByEmail(string email);
        Task<int> AddUser(CreateUserDTO dto);
        Task UpdateUser(int id,UpdateUserDTO dto);
        Task DeleteUser(int id);
    }
}
