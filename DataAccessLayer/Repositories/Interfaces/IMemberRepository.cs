using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllAsync();
        Task<List<Member>> GetAllMemberWithNameAsync(string name);
        Task<List<Member>> GetAllMemberWithEmailAsync(string email);
        Task<Member?> GetMemberByIdAsync(int id);
        Task<Member?> GetByUserIdAsync(int userId);
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(Member member);
        Task<int> TotalMembers();
    }
}
