using System.ComponentModel.DataAnnotations;

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
    }
}
