using Foodkart.DTOs.Main;

namespace Foodkart.DTOs.ViewDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public CartViewDto Carts { get; set; }
        public List<WishlistDto> Wishlists { get; set; }
    }
}
