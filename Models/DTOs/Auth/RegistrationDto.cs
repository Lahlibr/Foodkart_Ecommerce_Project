using System.ComponentModel.DataAnnotations;

namespace Foodkart.Models.DTOs.Auth
{
    public class RegistrationDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
