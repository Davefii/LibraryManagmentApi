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
    public class BorrowingService
    {
        private readonly BorrowingRepository _borrowingRepository;
        private readonly MemberService _memberService;
        private readonly BookRepository _bookRepository;
        private readonly AuditService _auditService;

        public BorrowingService(
            BorrowingRepository borrowingRepository,
            MemberService memberService,
            BookRepository bookRepository,
            AuditService auditService)
        {
            _borrowingRepository = borrowingRepository;
            _memberService = memberService;
            _bookRepository = bookRepository;
            _auditService = auditService;
        }

        public async Task<List<OverdueBorrowingDTO>>GetOverdueBorrowings()
        {
            var borrowings =
                await _borrowingRepository
                    .GetOverdueBorrowingsAsync();

            return borrowings.Select(b => new OverdueBorrowingDTO
            {
                BorrowingId = b.Id,
                BookTitle = b.Book.Title,
                DaysLate = (DateTime.UtcNow - b.DueDate).Days,
                MemberName = b.Member.Name,

                DueDate = b.DueDate,
            }).ToList();
        }

        public async Task<List<BorrowingResponseDTO>> GetAllBorrowings()
        {
            var borrowings =
                await _borrowingRepository.GetAllAsync();

            return borrowings.Select(b =>
                new BorrowingResponseDTO
                {
                    Id = b.Id,

                    MemberId = b.MemberId,

                    UserId = b.Member.UserId,

                    BookId = b.BookId,

                    BorrowDate = b.BorrowDate,

                    DueDate = b.DueDate,

                    ReturnDate = b.ReturnDate,

                    IsReturned = b.IsReturned,

                    CreatedAt = b.CreatedAt,

                    Book = new BookForReadOnlyDTOSmall
                    {
                        Id = b.Book.Id,
                        Title = b.Book.Title,
                        ISBN = b.Book.Isbn,
                        Description = b.Book.Description,
                        PublishYear = b.Book.PublishYear,
                        CoverImage = b.Book.CoverImage,
                    },

                    Member = new MemberForBorrowingsDTO
                    {
                        Id = b.Member.Id,
                        UserId = b.Member.UserId,
                        Name = b.Member.Name,
                        MembershipExpiryDate = b.Member.MembershipExpiryDate,
                        IsActive = b.Member.IsActive,
                    },

                    User = new UserResponseDTO
                    {
                        Id = b.Member.User.Id,
                        Email = b.Member.User.Email,
                        Role = b.Member.User.Role,
                        CreatedAt = b.Member.User.CreatedAt,
                        IsActive = b.Member.User.IsActive
                    }

                }).ToList();
        }

        public async Task<BorrowingResponseDTO?>GetBorrowingById(int id)
        {
            var borrowing =
                await _borrowingRepository.GetByIdAsync(id);

            if (borrowing == null)
                return null;

            return new BorrowingResponseDTO
            {
                Id = borrowing.Id,

                MemberId = borrowing.MemberId,

                UserId = borrowing.Member.UserId,

                BookId = borrowing.BookId,

                BorrowDate = borrowing.BorrowDate,

                DueDate = borrowing.DueDate,

                ReturnDate = borrowing.ReturnDate,

                IsReturned = borrowing.IsReturned,

                CreatedAt = borrowing.CreatedAt,

                Book = new BookForReadOnlyDTOSmall
                {
                    Id = borrowing.Book.Id,
                    Title = borrowing.Book.Title,
                    ISBN = borrowing.Book.Isbn,
                    Description = borrowing.Book.Description,
                    PublishYear = borrowing.Book.PublishYear,
                    CoverImage = borrowing.Book.CoverImage,
                },

                Member = new MemberForBorrowingsDTO
                {
                    Id = borrowing.Member.Id,
                    UserId = borrowing.Member.UserId,
                    Name = borrowing.Member.Name,
                    MembershipExpiryDate = borrowing.Member.MembershipExpiryDate,
                    IsActive = borrowing.Member.IsActive,
                },

                User = new UserResponseDTO
                {
                    Id = borrowing.Member.User.Id,
                    Email = borrowing.Member.User.Email,
                    Role = borrowing.Member.User.Role,
                    CreatedAt = borrowing.Member.User.CreatedAt,
                    IsActive = borrowing.Member.IsActive
                }
            };
        }

        public async Task<List<BorrowingResponseDTO>> GetMemberBorrowings(int memberId)
        {
            var borrowings =
                await _borrowingRepository
                    .GetBorrowingsByMemberIdAsync(memberId);

            return borrowings.Select(b =>
                new BorrowingResponseDTO
                {
                    Id = b.Id,
                    MemberId = b.MemberId,
                    UserId = b.Member.UserId,
                    BookId = b.BookId,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate,
                    IsReturned = b.IsReturned,
                    CreatedAt = b.CreatedAt,
                    Book = new BookForReadOnlyDTOSmall
                    {
                        Id = b.Book.Id,
                        Title = b.Book.Title,
                        ISBN = b.Book.Isbn,
                        Description = b.Book.Description,
                        PublishYear = b.Book.PublishYear,
                        CoverImage = b.Book.CoverImage,
                    },
                    Member = new MemberForBorrowingsDTO
                    {
                        Id = b.Member.Id,
                        UserId = b.Member.UserId,
                        Name = b.Member.Name,
                        MembershipExpiryDate = b.Member.MembershipExpiryDate,
                        IsActive = b.Member.IsActive,
                    },
                    User = new UserResponseDTO
                    {
                        Id = b.Member.User.Id,
                        Email = b.Member.User.Email,
                        Role = b.Member.User.Role,
                        CreatedAt = b.Member.User.CreatedAt,
                        IsActive = b.Member.User.IsActive
                    }
                }).ToList();
        }

        public async Task AddBorrowing(
            CreateBorrowingDTO dto)
        {
            var memeber = await _memberService.IsMemberExist(dto.MemberId);
            var book = await _bookRepository.GetByIdAsync(dto.BookId);
            var activeBorrowing = await _borrowingRepository.GetActiveBorrowingAsync(dto.MemberId,dto.BookId);
            var activeCount = await _borrowingRepository.CountActiveBorrowingsAsync(dto.MemberId);

            if (activeBorrowing != null)
            {
                throw new Exception(
                    "Member already borrowed this book");
            }
            if (activeCount >= 5)
            {
                throw new Exception(
                    "Maximum 5 books allowed");
            }
            if (memeber == null)
            {
                throw new Exception("Member Not Found");
            }
            if (!memeber.IsActive)
            {
                throw new Exception("Member is Not Active");
            }
            if (book == null)
            {
                throw new Exception("Book Not Found");
            }
            if (book.AvailableCopies <= 0)
            {
                throw new Exception("You Don't Have Copies of Book");
            }

            var borrowing = new Borrowing
            {
                MemberId = dto.MemberId,
                BookId = dto.BookId,
                BorrowDate = dto.BorrowDate,
                DueDate = dto.DueDate,
                IsReturned = false,
                CreatedAt = DateTime.UtcNow
            };

            book.AvailableCopies--;
            if (book.AvailableCopies == 0)
            {
                book.IsAvailable = false;
            }
            await _borrowingRepository.AddAsync(borrowing);
            await _auditService.LogAsync(
            memeber.UserId,
            "BORROW_BOOK",
            "BORROWING",
            $"Member borrowed book {book.Title}");
            await _bookRepository.UpdateAsync(book);
        }
        
        public async Task UpdateBorrowing(int id,UpdateBorrowingDTO dto)
        {
            var borrowing =
                await _borrowingRepository.GetByIdAsync(id);

            if (borrowing == null)
                throw new Exception("Borrowing Not Found");

            borrowing.MemberId = dto.MemberId;
            borrowing.BookId = dto.BookId;

            await _borrowingRepository.UpdateAsync(borrowing);
        }

        public async Task ReturnBook(int borrowingId)
        {
            var borrowing =
                await _borrowingRepository.GetByIdAsync(borrowingId);

            if (borrowing == null)
                throw new Exception("Borrowing Not Found");

            if (borrowing.IsReturned)
                throw new Exception("Book Already Returned");

            var book =
                await _bookRepository.GetByIdAsync(borrowing.BookId);

            if (book == null)
                throw new Exception("Book Not Found");

            var member =
                await _memberService
                    .GetMemberById(
                        borrowing.MemberId);

            if (member == null)
                throw new Exception(
                    "Member Not Found");

            await _auditService.LogAsync(
            member.UserId,
            "RETURN_BOOK",
            "BORROWING",
            $"Member returned book {book.Title}");

            borrowing.IsReturned = true;
            borrowing.ReturnDate = DateTime.UtcNow;

            book.AvailableCopies++;

            book.IsAvailable = true;
            await _bookRepository.UpdateAsync(book);

            await _borrowingRepository.UpdateAsync(borrowing);
        }

        public async Task DeleteBorrowing(int id)
        {
            var borrowing =
                await _borrowingRepository.GetByIdAsync(id);

            if (borrowing == null)
                throw new Exception("Borrowing Not Found");

            await _borrowingRepository.DeleteAsync(borrowing);
        }

    }
}
