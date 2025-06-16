using static Foodkart.Models.Entities.Products.Product;

namespace Foodkart.DTOs.Products
{
    public class ProductViewDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal? OfferPrice { get; set; }
        public decimal? RealPrice { get; set; }
        public int InStock { get; set; } 
        public FoodType? Type { get; set; } = FoodType.Veg;
        public string? ImageUrl { get; set; }
    }
}
