using BusinessLayer.DTOs;
using BusinessLayer.Services.Interface;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IDashboardService _dashbaordService;
        public MemberService(
            IMemberRepository memberRepository,
            IUserRepository userRepository,
            IBorrowingRepository borrowingRepository,
            IDashboardService dashbaordService)
        {
            _memberRepository = memberRepository;
            _userRepository = userRepository;
            _borrowingRepository = borrowingRepository;
            _dashbaordService = dashbaordService;
        }

        public async Task<List<MemberResponseDTO>> GetAllMembers()
        {
            var members = await _memberRepository.GetAllAsync();

            //var results = members
            return members.Select(member => new MemberResponseDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                Name = member.Name,
                Phone = member.Phone,
                Address = member.Address,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive,
                User = new UserForMemberResponseDTO
                {
                    Id = member.User.Id,
                    Email = member.User.Email
                },
                ActiveBorrowings = member.Borrowings.Where(B => !B.IsReturned).Count(),
                TotalBorrowings = member.Borrowings.Count(),
            }).ToList();
        }
        public async Task<List<MemberResponseDTO>> GetAllMembersWithNameMember(string Name)
        {
            var members = await _memberRepository.GetAllMemberWithNameAsync(Name);

            return members.Select(member => new MemberResponseDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                Name = member.Name,
                Phone = member.Phone,
                Address = member.Address,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive,
                User = new UserForMemberResponseDTO
                {
                    Id = member.User.Id,
                    Email = member.User.Email
                },
                ActiveBorrowings = member.Borrowings.Where(B => !B.IsReturned).Count(),
                TotalBorrowings = member.Borrowings.Count(),
            }).ToList();
        }
        public async Task<List<MemberResponseDTO>> GetAllMembersWithUserEmail(string userEmail)
        {
            var members = await _memberRepository.GetAllMemberWithEmailAsync(userEmail);

            return members.Select(member => new MemberResponseDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                Name = member.Name,
                Phone = member.Phone,
                Address = member.Address,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive,
                User = new UserForMemberResponseDTO
                {
                    Id = member.User.Id,
                    Email = member.User.Email
                },
                ActiveBorrowings = member.Borrowings.Where(B => !B.IsReturned).Count(),
                TotalBorrowings = member.Borrowings.Count(),
            }).ToList();
        }
        public async Task<MemberResponseDTO?> GetMemberById(int id)
        {
            var member = await _memberRepository.GetMemberByIdAsync(id);

            if (member == null)
                return null;

            return new MemberResponseDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                Name = member.Name,
                Phone = member.Phone,
                Address = member.Address,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive,
                User = new UserForMemberResponseDTO
                {
                    Id = member.User.Id,
                    Email = member.User.Email
                },
                ActiveBorrowings = member.Borrowings.Where(B => !B.IsReturned).Count(),
                TotalBorrowings = member.Borrowings.Count(),
            };
        }
        public async Task<MemberForBorrowingsDTO?> IsMemberExist(int id)
        {
            var member = await _memberRepository.GetMemberByIdAsync(id);

            if (member == null)
                return null;

            return new MemberForBorrowingsDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                Name = member.Name,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive
            };
        }
        public async Task AddMember(int userId, CreateMemberDTO dto)
        {
            var existingMember =
                    await _memberRepository
                        .GetByUserIdAsync(userId);

            if (existingMember != null)
            {
                throw new Exception(
                    "You are already a member");
            }

            var member = new Member
            {
                UserId = userId,
                Name = dto.Name,
                Phone = dto.Phone,
                Address = dto.Address,
                MembershipExpiryDate =
                    dto.MembershipExpiryDate,
                IsActive = true
            };
            await _memberRepository.AddAsync(member);
        }

        public async Task UpdateMember(
            int id,
            UpdateMemberDTO dto)
        {
            var member =
                await _memberRepository.GetMemberByIdAsync(id);

            if (member == null)
                throw new Exception("Member Not Found");

            if (!string.IsNullOrWhiteSpace(dto.Name))
                member.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Phone))
                member.Phone = dto.Phone;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                member.Address = dto.Address;

            await _memberRepository.UpdateAsync(member);
        }

        public async Task DeleteMember(int id)
        {
            var member =
                await _memberRepository.GetMemberByIdAsync(id);

            if (member == null)
                throw new Exception("Member Not Found");

            await _memberRepository.DeleteAsync(member);
        }

        public async Task<MemberResponseDTO> GetByUserId(int currentUserId)
        {
            var member = await _memberRepository.GetByUserIdAsync(currentUserId);

            if (member == null)
                return null;

            return new MemberResponseDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                Name = member.Name,
                Phone = member.Phone,
                Address = member.Address,
                MembershipExpiryDate = member.MembershipExpiryDate,
                IsActive = member.IsActive,
                User = new UserForMemberResponseDTO
                {
                    Id = member.User.Id,
                    Email = member.User.Email
                }
            };
        }
    }
}