using static Foodkart.Models.Entities.Products.Product;

namespace Foodkart.DTOs.Products
{
    public class ProductViewDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Offer_Price { get; set; }
        public decimal? Real_Price { get; set; }
        public int InStock { get; set; } 
        public FoodType? Type { get; set; } = FoodType.Veg;
        public string? ImageUrl { get; set; }
    }
}
