using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Requests
{
    public class CreateAuthorRequest
    {
        [Required][MaxLength(100)] public string FirstName { get; set; } = null!;
        [Required][MaxLength(100)] public string LastName { get; set; } = null!;
        [MaxLength(1000)] public string? Biography { get; set; }
        [MaxLength(100)] public string? Nationality { get; set; }
        public IFormFile? ImageAuthor { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}
