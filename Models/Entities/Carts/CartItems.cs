using Foodkart.Models.Entities.Products;
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
        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }
    }
}
