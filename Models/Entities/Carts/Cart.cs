namespace Foodkart.Models.Entities.Carts
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public  Main.User ? User { get; set; }
        // Navigation property for related CartItems
        public virtual ICollection<CartItems>? CartItems { get; set; }
        
    }
}
