using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Foodkart.Models.Entities
{
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string PasswordHash { get; set; }
        [Required]
        public Guid Salt { get; set; }
        [Required]
        public bool EmailVerified { get; set; } = false;
        public Guid? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpiry { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        [Required]
        public bool Blocked { get; set; } = false;
        [Required]
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;




    }
}
