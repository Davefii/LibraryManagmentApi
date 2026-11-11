using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IMemberService
    {
        Task<List<MemberResponseDTO>> GetAllMembers();
        Task<List<MemberResponseDTO>> GetAllMembersWithNameMember(string name);
        Task<List<MemberResponseDTO>> GetAllMembersWithUserEmail(string email);
        Task<MemberResponseDTO?> GetMemberById(int id);
        Task<MemberForBorrowingsDTO?> IsMemberExist(int id);
        Task AddMember(int userId, CreateMemberDTO dto);
        Task UpdateMember(int id, UpdateMemberDTO dto);
        Task DeleteMember(int id);
        Task<MemberResponseDTO> GetByUserId(int currentUserId);
    }
}
