using Microsoft.AspNetCore.Http;
namespace LibraryApi.Requests
{
    public class CreateBookRequest
    {
        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int PublishYear { get; set; }

        public int CopiesCount { get; set; }

        public IFormFile? CoverImage { get; set; }
    }
}
