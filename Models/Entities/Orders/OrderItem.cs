using System.ComponentModel.DataAnnotations.Schema;
using Foodkart.Models.Entities.Products;

namespace Foodkart.Models.Entities.Orders
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int productId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalPrice { get; set; }
        public int Quantity { get; set; }
        public virtual Product? Product { get; set; }

       
    }
}
