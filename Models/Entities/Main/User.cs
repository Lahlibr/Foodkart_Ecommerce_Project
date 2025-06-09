using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Foodkart.Models.Entities.Orders;
using Foodkart.Models.Entities.Carts;
using static Foodkart.Models.Entities.Base.TimeStamp;


namespace Foodkart.Models.Entities.Main
{
    public class User : TimeStampEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter proper Email")]
        
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.[A-Za-z])(?=.\d)(?=.[@$!%?&])[A-Za-z\d@$!%*?&]{8,}$",
         ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
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


        public bool Deleted { get; set; } = false;
        public string Role { get; set; } = "User";
        public virtual Cart? Carts { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
        
        
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        


        //virtual Enables lazy loading in EF Core(if enabled)
        //ICollection	Represents a collection for one-to-many relationships


    
    }
}
