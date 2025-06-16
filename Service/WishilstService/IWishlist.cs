using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;

namespace Foodkart.Service.WishlistService

{
    public interface IWishlist 
    {
        public Task<ApiResponse<WishlistViewDto>> AddToWishlist(int userId, int productId);
        public Task<ApiResponse<List<WishlistViewDto>>> GetWishlist(int userId);
    }
}
