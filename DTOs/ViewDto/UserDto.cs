using Foodkart.DTOs.AddDto;


namespace Foodkart.DTOs.ViewDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public CartViewDto Carts { get; set; }

        public List<WishlistViewDto> Wishlists { get; set; }
        public string Role { get; set; }
        public List<AddressDto> Addresses { get; set; }
        public bool Blocked { get; set; } = false;
    }
}
