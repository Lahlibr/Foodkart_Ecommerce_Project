using System.ComponentModel.DataAnnotations;

namespace Foodkart.Models.Entities
{
    public class Login
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public Guid Salt { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
    }
}
