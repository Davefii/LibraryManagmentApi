using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace LibraryApi.Requests
{
    public class CreateBookRequest
    {
        [Required] public string Title { get; set; }
        [Required] public string ISBN { get; set; }
        [Required] public string Description { get; set; }
        [Required] public int PublishYear { get; set; }
        [Required] public int TotalCopies { get; set; }
        [Required] public int AvailableCopies { get; set; }
        [Required] public bool IsAvailable { get; set; }
        [Required]  public IFormFile? CoverImage { get; set; }
        [Required] public int AuthorID { get; set; }
        [Required] public int CategoryID { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public DateTime? UpdatedAt { get; set; }
    }
}
