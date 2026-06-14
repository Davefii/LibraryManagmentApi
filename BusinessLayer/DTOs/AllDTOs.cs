// =============================================
// Library Management System — All DTOs
// Folder: BusinessLayer/DTOs/
// =============================================

using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs
{
    // AUTH DTOs
    public class RegisterRequestDTO
    {
        [Required][EmailAddress][MaxLength(150)]
        public string Email { get; set; } = null!;
        [Required][MinLength(8)]
        public string Password { get; set; } = null!;
        [Required][MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [Required][MaxLength(100)]
        public string LastName { get; set; } = null!;
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
    }

    public class LoginRequestDTO
    {
        [Required][EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class AuthResponseDTO
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public UserResponseDTO User { get; set; } = null!;
    }

    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;
        [Required][MinLength(8)]
        public string NewPassword { get; set; } = null!;
    }

    // USER DTOs

    public class Roles
    {
        public const string Admin = "Admin";

        public const string Member = "Member";
    }
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
        public string Role { get; set; } = Roles.Member;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    //UserProfile
    public class CreateUserProfileDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
    public class UpdateUserProfileDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
    public class UserProfileResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
    // AUTHOR DTOs
    public class CreateAuthorDTO
    {
        [Required][MaxLength(100)] public string FirstName { get; set; } = null!;
        [Required][MaxLength(100)] public string LastName { get; set; } = null!;
        [MaxLength(1000)]          public string? Biography { get; set; }
        [MaxLength(100)]           public string? Nationality { get; set; }
        public DateOnly? BirthDate { get; set; }
    }

    public class UpdateAuthorDTO
    {
        [MaxLength(100)] public string? FirstName { get; set; }
        [MaxLength(100)] public string? LastName { get; set; }
        [MaxLength(1000)] public string? Biography { get; set; }
        [MaxLength(100)] public string? Nationality { get; set; }
        public DateOnly? BirthDate { get; set; }
    }

    public class AuthorResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Biography { get; set; }
        public string? Nationality { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int TotalBooks { get; set; }
    }
    public class AuthorSummaryDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
    // CATEGORY DTOs
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? ParentId { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? ParentId { get; set; }
    }
    public class CategoryResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? ParentId { get; set; }
    }
    //public partial class Category
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string? Description { get; set; }
    //    public int? ParentId { get; set; }
    //    public virtual Category? Parent { get; set; }
    //    public virtual ICollection<Category> InverseParent { get; set; }
    //    public virtual ICollection<Book> Books { get; set; }
    //}

    // BOOK DTOs
    public class BookForReadOnlyDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ISBN { get; set; }

        public string Description { get; set; }

        public int? PublishYear { get; set; }

        public List<AuthorSummaryDTO> Authors { get; set; }
            = new();
        public List<CategorySummaryDTO> Categories { get; set; } = new();

    }
    public class CategorySummaryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
    public class CreateBookDTO
    {
        public int Id { get;}
        [Required][MaxLength(200)] public string Title { get; set; } = null!;
        [Required][MaxLength(50)]  public string ISBN { get; set; } = null!;
        [MaxLength(1000)]          public string? Description { get; set; }
        public int? PublishYear { get; set; }
        [Required] public int CopiesCount { get; set; }
        [Required] public List<int> AuthorIds { get; set; } = new();
        [Required] public List<int> CategoryIds { get; set; } = new();
    }

    public class UpdateBookDTO
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public int PublishYear { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable { get; set; }
        public List<int>? AuthorIds { get; set; }
        public List<int>? CategoryIds { get; set; }
    }

    public class BookResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string? Description { get; set; }
        public int? PublishYear { get; set; }
        public int CopiesCount { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable { get; set; }
        public List<AuthorSummaryDTO> Authors { get; set; } = new();
        public List<CategorySummaryDTO> Categories { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    // MEMBER DTOs
    public class CreateMemberDTO
    {
        [Required][MaxLength(150)] public string Name { get; set; } = null!;
        [MaxLength(20)]  public string? Phone { get; set; }
        [MaxLength(200)] public string? Address { get; set; }
        [Required] public DateTime MembershipExpiryDate { get; set; }
        [Required] public int UserID { get; set; }
    }

    public class UpdateMemberDTO
    {
        [MaxLength(150)] public string? Name { get; set; }
        [MaxLength(20)]  public string? Phone { get; set; }
        [MaxLength(200)] public string? Address { get; set; }
        public DateTime? MembershipExpiryDate { get; set; }
    }

    public class MemberResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime MembershipExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public int TotalBorrowings { get; set; }
        public int ActiveBorrowings { get; set; }
    }
    public class MemberForBorrowingsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime MembershipExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
    // BORROWING DTOs
    public class CreateBorrowingDTO
    {
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate = DateTime.UtcNow.AddDays(14);
    }
    public class UpdateBorrowingDTO
    {
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsReturned { get; set; }
    }
    public class BorrowingResponseDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class OverdueBorrowingDTO
    {
        public int BorrowingId { get; set; }

        public string MemberName { get; set; } = null!;

        public string BookTitle { get; set; } = null!;

        public DateTime DueDate { get; set; }

        public int DaysLate { get; set; }
    }
    // DASHBOARD DTO
    public class DashboardDTO
    {
        public int TotalBooks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalCategories { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveBorrowings { get; set; }
        public int OverdueBorrowings { get; set; }
        public int BooksUnavailable { get; set; }
        //public List<TopBookDTO> TopBooks { get; set; } = new();
        //public List<TopMemberDTO> TopMembers { get; set; } = new();
    }

    public class TopBookDTO
    {
        public string Title { get; set; } = null!;
        public int TotalBorrowings { get; set; }
    }

    public class TopMemberDTO
    {
        public string Name { get; set; } = null!;
        public int TotalBorrowings { get; set; }
    }

    // PAGINATION
    public class PagedResultDTO<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
