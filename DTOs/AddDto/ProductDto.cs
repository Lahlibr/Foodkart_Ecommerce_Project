using System.ComponentModel.DataAnnotations;
using Foodkart.Models.Enum;

namespace Foodkart.DTOs.AddDto
{
    public class ProductDto
    {
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? RealPrice { get; set; }
        public decimal? OfferPrice { get; set; }

        [Required]
        public int InStock { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        [Required]
        public FoodType Type { get; set; } 
        
    }
}
