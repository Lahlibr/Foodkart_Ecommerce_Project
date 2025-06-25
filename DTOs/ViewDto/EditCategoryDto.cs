using System.ComponentModel.DataAnnotations;

namespace Foodkart.DTOs.ViewDto
{
    public class EditCategoryDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name must be under 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description can be up to 500 characters.")]
        public string? Description { get; set; }
    }
}
