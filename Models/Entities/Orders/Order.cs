using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Foodkart.Models.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Models.Entities.Orders
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int AddressId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        [Required]
        public string? TransactionId { get; set; }

        public virtual User? User { get; set; }
        public virtual Address? Address { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public string Status { get; set; }
    }
}
