using Foodkart.DTOs.ViewDto;

namespace Foodkart.DTOs.Auth
{
    public class ResultDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Error { get; set; }
        public List<CartItemViewDto> Carts { get; set; } = new();
        public string Wishlists { get; set; } 
    }
}
