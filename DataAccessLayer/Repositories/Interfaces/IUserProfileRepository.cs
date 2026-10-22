using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<List<UserProfile>> GetAllAsync();
        Task<UserProfile?> GetByIdAsync(int id);
        Task<UserProfile?> GetByUserIdAsync(int userId);
        Task AddAsync(UserProfile profile);
        Task UpdateAsync(UserProfile profile);
        Task DeleteAsync(UserProfile profile);
    }
}
