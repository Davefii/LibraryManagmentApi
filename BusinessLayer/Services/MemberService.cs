using BusinessLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class MemberService
    {
        private readonly MemberRepository _memberRepository;
        private readonly UserRepository _userRepository;

        public MemberService(
            MemberRepository memberRepository,
            UserRepository userRepository)
        {
            _memberRepository = memberRepository;
            _userRepository = userRepository;
        }

        public async Task<List<MemberResponseDTO>> GetAllMembers()
        {
            var members = await _memberRepository.GetAllAsync();

            return members.Select(member => new MemberResponseDTO
            {
                Id = member.Id,
                Name = member.Name,
                Phone = member.Phone,
                Address = member.Address,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive
            }).ToList();
        }

        public async Task<MemberResponseDTO?> GetMemberById(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
                return null;

            return new MemberResponseDTO
            {
                Id = member.Id,
                Name = member.Name,
                Phone = member.Phone,
                Address = member.Address,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive
            };
        }
        public async Task<MemberForBorrowingsDTO?> IsMemberExist(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
                return null;

            return new MemberForBorrowingsDTO
            {
                Id = member.Id,
                Name = member.Name,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive
            };
        }
        public async Task AddMember(CreateMemberDTO dto)
        {
            var user =
                await _userRepository.GetByIdAsync(dto.UserID);

            if (user == null)
                throw new Exception("User Not Found");

            var existingMember =
                await _memberRepository.GetByUserIdAsync(dto.UserID);

            if (existingMember != null)
                throw new Exception("User already has a member account");

            var member = new Member
            {
                UserId = dto.UserID,
                Name = dto.Name,
                Phone = dto.Phone ?? "",
                Address = dto.Address ?? "",
                MembershipExpiryDate = dto.MembershipExpiryDate,
                IsActive = true
            };

            await _memberRepository.AddAsync(member);
        }

        public async Task UpdateMember(
            int id,
            UpdateMemberDTO dto)
        {
            var member =
                await _memberRepository.GetByIdAsync(id);

            if (member == null)
                throw new Exception("Member Not Found");

            if (!string.IsNullOrWhiteSpace(dto.Name))
                member.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Phone))
                member.Phone = dto.Phone;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                member.Address = dto.Address;

            if (dto.MembershipExpiryDate.HasValue)
                member.MembershipExpiryDate =
                    dto.MembershipExpiryDate.Value;

            await _memberRepository.UpdateAsync(member);
        }

        public async Task DeleteMember(int id)
        {
            var member =
                await _memberRepository.GetByIdAsync(id);

            if (member == null)
                throw new Exception("Member Not Found");

            await _memberRepository.DeleteAsync(member);
        }
    }
}