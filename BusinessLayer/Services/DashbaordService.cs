using DataAccessLayer.Repositories.DTOs;
using BusinessLayer.Services.Interface;
using DataAccessLayer.Context;
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
    public class DashbaordService : IDashboardService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IDashboardRepository _dashboardRepository;
        public DashbaordService(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IMemberRepository memberRepository,
            IBorrowingRepository borrowingRepository,
            IDashboardRepository dashboardRepository
            )
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _memberRepository = memberRepository;
            _borrowingRepository = borrowingRepository;
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardDTO> Dashboard()
        {
            return new DashboardDTO
            {
                TotalBooks = await _bookRepository.TotalBooks(),

                TotalAuthors = await _authorRepository.TotalAuthors(),

                TotalCategories = await _categoryRepository.TotalCategory(),

                TotalMembers = await _memberRepository.TotalMembers(),

                ActiveBorrowings = await _borrowingRepository.TotalBorrowings(),

                OverdueBorrowings = await _borrowingRepository.GetOverdueBorrowingsCountAsync(),

                BooksUnavailable = await _bookRepository.GetUnavailableBooksCountAsync()
            };


        }
        public async Task<List<PopularBookDTO>> GetPopularBooksAsync()
        {
            return await _dashboardRepository.GetPopularBooksReturnedAsync();
        }
        public async Task<List<PopularBookDTO>> GetPopularBooksReturnedAsync()
        {
            return await _dashboardRepository.GetPopularBooksReturnedAsync();
        }
        public async Task<List<BooksbycategoryDTO>> GetBooksByCategoryAsync()
        {
            return await _dashboardRepository.GetBooksByCategoryAsync();
        }
        public async Task<List<RecentborrowingsDTO>> GetRecentborrowingsAsync()
        {
            var Recentborrowings = await _borrowingRepository.Recentborrowings();
            return Recentborrowings.Select(RB => new RecentborrowingsDTO
            {
                BorrowingId = RB.Id,
                MemberName = RB.Member.Name,
                BookTitle = RB.Book.Title,
                Status = RB.IsReturned ? "Returned" : "Active",
                Datee = RB.BorrowDate <= DateTime.Now ? RB.BorrowDate : DateTime.Now,
            }).ToList();
        }
        public async Task<int> TotalBorrowingsForSelfMemberAsync(int MemberID)
        {
            return await _borrowingRepository.TotalBorrowingsForSelfMemberAsync(MemberID);
        }
        public async Task<int> TotalActiveBorrowingsForSelfMemberAsync(int MemberID)
        {
            return await _borrowingRepository.TotalActiveBorrowingsForSelfMemberAsync(MemberID);
        }

    }
}
