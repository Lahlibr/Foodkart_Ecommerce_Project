using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;

namespace Foodkart.Service.CartService
{
    public interface ICartService
    {
        Task<ApiResponse<CartViewDto>> AddToCart(int userId, int productId, int quantity);
        Task<ApiResponse<CartViewDto>>GetCartItems(int userId);
        Task<ApiResponse<CartViewDto>> RemoveFromCart(int userId, int productId);
        Task<ApiResponse<CartViewDto>> AddQuantity(int userId, int productId, int quantity);
        Task<ApiResponse<CartViewDto>> ReduceQuantity(int userId, int productId, int quantity);
        Task<ApiResponse<CartViewDto>> ClearCart(int userId);
        Task<ApiResponse<List<CartViewDto>>> AllUsersCart(int productId);

    }
}
