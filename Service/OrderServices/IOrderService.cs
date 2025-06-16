using Foodkart.DTOs.AddDto;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;

namespace Foodkart.Service.OrderServices
{
    public interface IOrderService
    {
        Task<ApiResponse<CartViewDto>> CreateOrderAsync(int userId);
        Task<ApiResponse<List<OrderViewDto>>> GetOrders(int userId);
        Task<List<AdminViewOrderDto>> GetOrdersforAdmin(int userId);
        Task<int> TotalProductSold();
        Task<decimal?> TotalRevenue();
        Task<ApiResponse<string>> UpdateOrderStatus(int orderId, string newStatus);
    }
}
