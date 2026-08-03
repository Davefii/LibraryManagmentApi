namespace LibraryApi.Requests
{
    public class UpdateBookRequest
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public int PublishYear { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
