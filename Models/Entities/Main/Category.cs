using System.ComponentModel.DataAnnotations;
using Foodkart.Models.Entities.Products;

namespace Foodkart.Models.Entities.Main
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; } = new HashSet<Product>();
    }
}
