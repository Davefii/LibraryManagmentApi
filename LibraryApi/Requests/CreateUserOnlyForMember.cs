using BusinessLayer.DTOs;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Requests
{
    public class CreateUserOnlyForMember
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
    }
}
