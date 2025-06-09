using System.ComponentModel.DataAnnotations;
using Foodkart.Models.Entities.Carts;
using Foodkart.Models.Entities.Main;
using Microsoft.EntityFrameworkCore;
using static Foodkart.Models.Entities.Base.TimeStamp;

namespace Foodkart.Models.Entities.Products
{
    public class Product : TimeStampEntity
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Offer_Price { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Real_Price { get; set; }
        public string? ImageUrl { get; set; } = null;
        public int InStock { get; set; } 
        public enum FoodType {
        Veg,NonVeg}
        [Required]
        public FoodType Type { get; set; } = FoodType.Veg;
        [Required]
        public int CategoryId { get; set; }
        public virtual Category? category { get; set; }
        // to Querying all carts this product is in. related to it
        public virtual List<CartItems>? CartItems { get; set; }
    }
}
