using Microsoft.AspNetCore.Http;
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

    public class TokenResponseDTO
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }

    public class LoginRequestDTO
    {
        [Required][EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class RefreshRequestDTO
    {
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string Email { get; set; }
    }

    public class LogoutRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RefreshToken { get; set; }
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
        public string Password { get; set; } = null!;
        public string Role { get; set; } = Roles.Member;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class UserResponseDTO
    {
        public int Id { get; set; }
        [Required] public string Email { get; set; } = null!;
        [Required] public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateUserDTO
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; }
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
        public UserResponseDTO User { get; set; }
    }
    // AUTHOR DTOs
    public class CreateAuthorDTO
    {
        [Required][MaxLength(100)] public string FirstName { get; set; } = null!;
        [Required][MaxLength(100)] public string LastName { get; set; } = null!;
        [MaxLength(1000)]          public string? Biography { get; set; }
        [MaxLength(100)]           public string? Nationality { get; set; }
        public string? ImageAuthor { get; set; }
        public DateOnly? BirthDate { get; set; }
    }

    public class UpdateAuthorDTO
    {
        [MaxLength(100)] public string? FirstName { get; set; }
        [MaxLength(100)] public string? LastName { get; set; }
        [MaxLength(1000)] public string? Biography { get; set; }
        [MaxLength(100)] public string? Nationality { get; set; }
        public string? ImageAuthor { get; set; }
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
        public string? ImageAuthor { get; set; }
        public int TotalBooks { get; set; }
        public virtual IEnumerable<Book> Books { get; set; } = new List<Book>();
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

        public  List<BookResponseDTO> Books { get; set; } = new();
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
    public class BookForReadOnlyDTOSmall
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ISBN { get; set; }

        public string Description { get; set; }

        public int? PublishYear { get; set; }

        public string? CoverImage { get; set; }

    }

    public class BookForReadOnlyDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string? Description { get; set; }
        public int? PublishYear { get; set; }
        public int CopiesCount { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable { get; set; }

        public string? CoverImage { get; set; }

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
        [Required]  public int Id { get;}
        [Required]  public string Title { get; set; }
        [Required]  public string ISBN { get; set; }
        [Required]  public string Description { get; set; }
        [Required]  public int PublishYear { get; set; }
        [Required]  public int TotalCopies { get; set; }
        [Required]  public int AvailableCopies { get; set; }
        [Required]  public bool IsAvailable { get; set; }
        [Required]   public string CoverImage { get; set; }
        [Required] public int AuthorID { get; set; }
        [Required] public int CategoryID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateBookDTO
    {
        [Required] public string Title { get; set; }
        [Required] public string ISBN { get; set; }
        [Required] public string Description { get; set; }
        [Required] public int PublishYear { get; set; }
        [Required] public int TotalCopies { get; set; }
        [Required] public int AvailableCopies { get; set; }
        [Required] public bool IsAvailable { get; set; }
        [Required] public string? CoverImage { get; set; }
        [Required] public int AuthorID { get; set; }
        [Required] public int CategoryID { get; set; }
        public DateTime? UpdatedAt { get; set; }
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
        public string? CoverImage { get; set; }
        public List<AuthorSummaryDTO> Authors { get; set; } = new();
        public List<CategorySummaryDTO> Categories { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // MEMBER DTOs
    public class CreateMemberDTO
    {
        [Required][MaxLength(150)] public string Name { get; set; } = null!;
        [Required][MaxLength(20)]  public string? Phone { get; set; }
        [Required][MaxLength(200)] public string? Address { get; set; }
        [Required] public DateTime MembershipExpiryDate { get; set; }
        //[Required] public int UserID { get; set; }
    }

    public class UpdateMemberDTO
    {
        [Required][MaxLength(150)] public string? Name { get; set; }
        [Required][MaxLength(20)]  public string? Phone { get; set; }
        [Required][MaxLength(200)] public string? Address { get; set; }
        //public DateTime? MembershipExpiryDate { get; set; }
    }

    public class MemberResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime MembershipExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public int TotalBorrowings { get; set; }
        public int ActiveBorrowings { get; set; }
        public UserForMemberResponseDTO? User { get; set; }
    }
    public class MemberForBorrowingsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime MembershipExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class UserForMemberResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
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
        public int MemberId { get; set; }
        public int BookId { get; set; }
    }
    public class BorrowingResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookForReadOnlyDTOSmall? Book { get; set; }
        public MemberForBorrowingsDTO? Member { get; set; }
        public UserResponseDTO? User { get; set; }
    }
    public class OverdueBorrowingDTO
    {
        public int BorrowingId { get; set; }

        public string MemberName { get; set; } = null!;

        public string BookTitle { get; set; } = null!;

        public DateTime DueDate { get; set; }

        public int DaysLate { get; set; }
    }
    public class RecentborrowingsDTO
    {
        public int BorrowingId { get; set; }
        public string MemberName { get; set; }
        public string BookTitle { get; set; }
        public string Status { get; set; }
        public DateTime Datee { get; set; }
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
    public class PopularBookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int BorrowCount { get; set; }
    }
    public class BooksbycategoryDTO
    {
        public string NameCategory { get; set; } = null!;
        public int TotalBooks { get; set; }
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
    // AUDIT DTOs
    public class AuditLogResponseDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Action { get; set; } = null!;
        public string EntityName { get; set; } = null!;
        public string Details { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
