using Foodkart.Models.Entities.Products;

namespace Foodkart.Models.Entities.Main
{
    public class Wishlist
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public virtual User ? users { get; set; }
        public virtual Product? products { get; set; }
    }
}
