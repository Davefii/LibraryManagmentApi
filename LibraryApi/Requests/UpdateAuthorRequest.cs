using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Requests
{
    public class UpdateAuthorRequest
    {
        [MaxLength(100)] public string? FirstName { get; set; }
        [MaxLength(100)] public string? LastName { get; set; }
        [MaxLength(1000)] public string? Biography { get; set; }
        [MaxLength(100)] public string? Nationality { get; set; }
        [Required] public IFormFile? ImageAuthor { get; set; }
        [Required] public DateOnly? BirthDate { get; set; }
        [Required] public string AuthorName { get; set; }
        [Required] public string Category { get; set; }
    }
}
