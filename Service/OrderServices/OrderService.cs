using Foodkart.Models.Entities.Main;
using Foodkart.Models.Entities.Orders;
using Foodkart.DTOs.ViewDto;
using Foodkart.DTOs.AddDto;
using Foodkart.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Foodkart.Models.Entities.Carts;
using Amazon.IdentityManagement.Model;
namespace Foodkart.Service.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly FoodkartDbContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;
        public OrderService(FoodkartDbContext context, ILogger<OrderService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ApiResponse<string>> CreateOrderAsync(int userId, int addressId)
        {
            try
            {
                // Verify the address exists and belongs to the user
                var addressExists = await _context.Addresses
                    .AnyAsync(a => a.AddressId == addressId && a.UserId == userId);

                if (!addressExists)
                {
                    return new ApiResponse<string>(400, "Invalid address ID or address doesn't belong to user");
                }

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                {
                    return new ApiResponse<string>(404, "Cart is empty.");
                }

                string transactionId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12).ToUpper();
                decimal totalAmount = cart.CartItems.Sum(ci => ci.Product.OfferPrice * ci.Quantity);

                var newOrder = new Order
                {
                    UserId = userId,
                    AddressId = addressId, // Add this line
                    TransactionId = transactionId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        productId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Product.OfferPrice * ci.Quantity
                    }).ToList()
                };

                await _context.Orders.AddAsync(newOrder);
                _context.CartItems.RemoveRange(cart.CartItems);

                await _context.SaveChangesAsync();

                return new ApiResponse<string>(200, "Order placed successfully", transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while placing the order");
                return new ApiResponse<string>(500, $"Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<List<OrderViewDto>>> GetOrders(int userid)
        {

            var userOrders = await _context.Orders
                            .Include(x => x.OrderItems)
                            .ThenInclude(x => x.Product)
                            .Where(x => x.UserId == userid)
                            .ToListAsync();

            var deliveryAddresses = await _context.Addresses
                .Where(x => x.UserId == userid)
                .ToListAsync();


            var addressDict = deliveryAddresses
                .GroupBy(addr => addr.UserId)
                .ToDictionary(group => group.Key, group => group.FirstOrDefault());


            var orderRes = userOrders.Select(order => new OrderViewDto
            {
                TransactionId = order.TransactionId,
                TotalAmount = order.TotalAmount,
                DeliveryAddress = addressDict.TryGetValue(order.UserId, out var address)
                    ? $" {address.HouseName}, {address.Pincode} {address.LandMark}, {address.Place}"
                    : "Address not found",
                Phone = addressDict.TryGetValue(order.UserId, out var phoneAddress) ? phoneAddress.PhoneNumber : "Phone not available",
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(orderItem => new CreateOrderItemDto
                {
                    ProductName = orderItem.Product.ProductName,
                    TotalPrice = orderItem.TotalPrice,
                    Quantity = orderItem.Quantity
                }).ToList()
            }).ToList();

            return new ApiResponse<List<OrderViewDto>>(200, "Successfully Fetched User Orders", orderRes);

        }

        public async Task<List<AdminViewOrderDto>> GetOrdersforAdmin(int userid)
        {
            var userorders = await _context.Users
                .Include(c => c.Orders)
                .ThenInclude(n => n.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == userid);
            if (userorders == null)
            {
                return null;
            }
            var result = userorders.Orders.Select(item => new AdminViewOrderDto
            {
                OrderId = item.OrderId,
                TransactionId = item.TransactionId,
                TotalAmount = (decimal)item.TotalAmount,
                OrderDate = item.OrderDate
            }).ToList();
            return result;
        }




        public async Task<int> TotalProductSold()
        {
            try
            {
                int sales = await _context.OrderItems.Where(oi => oi.RelatedOrder.Status == "Delivered").SumAsync(x => x.Quantity);
                return sales;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<decimal?> TotalRevenue()
        {
            try
            {
                var total = await _context.OrderItems.Where(oi => oi.RelatedOrder.Status == "Delivered").SumAsync(x => x.TotalPrice);
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<string>> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return new ApiResponse<string>(404, "Order not found.");
            }

            order.Status = newStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Order status updated successfully.");
        }
    }
}

