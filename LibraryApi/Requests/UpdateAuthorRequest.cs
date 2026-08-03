using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Requests
{
    public class UpdateAuthorRequest
    {
        [MaxLength(100)] public string? FirstName { get; set; }
        [MaxLength(100)] public string? LastName { get; set; }
        [MaxLength(1000)] public string? Biography { get; set; }
        [MaxLength(100)] public string? Nationality { get; set; }
        public IFormFile? ImageAuthor { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}
