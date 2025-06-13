using Foodkart.Models.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Foodkart.Models.Entities.Carts
{
    public class CartItems
    {
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }
    }
}
