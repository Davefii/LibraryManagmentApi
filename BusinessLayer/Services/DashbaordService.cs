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
    public class DashbaordService
    {
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly MemberRepository _memberRepository;
        private readonly BorrowingRepository _borrowingRepository;

        public DashbaordService(
            BookRepository bookRepository,
            AuthorRepository authorRepository,
            CategoryRepository categoryRepository,
            MemberRepository memberRepository,
            BorrowingRepository borrowingRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _memberRepository = memberRepository;
            _borrowingRepository = borrowingRepository;
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

                OverdueBorrowings = await _borrowingRepository .GetOverdueBorrowingsCountAsync(),

                BooksUnavailable = await _bookRepository.GetUnavailableBooksCountAsync()
            };


        }
    }
}
