using System.Security.Claims;
using Amazon.IdentityManagement.Model;
using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.AddDto;
using Foodkart.DTOs.ViewDto;
using Foodkart.Service.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodkart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly FoodkartDbContext _context;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger, IMapper mapper, FoodkartDbContext context)
        {
            _orderService = orderService;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        [HttpPost("Place")]
        [Authorize]
        public async Task<IActionResult> PlaceOrder([FromBody]CreateOrderRequestDto orderDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized("Invalid or missing user ID in token.");
                }

                var response = await _orderService.CreateOrderAsync(orderDto.UserId, orderDto.AddressId);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to place order.");
                return StatusCode(500, $"An error occurred while placing the order: {ex.Message}");
            }
        }

        [HttpGet("UserOrderDetails/{userid}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int userid)
        {
            try
            {
                var orders = await _orderService.GetOrdersforAdmin(userid);
                if (orders == null || orders.Count == 0)
                {
                    return Ok(new List<OrderViewDto>()); // Or whatever DTO you use
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var result = await _orderService.GetOrders(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("sales")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TotalSale()
        {
            try
            {
                var result = await _orderService.TotalProductSold();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("totalrevenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TotalRevenue()
        {
            try
            {
                var total = await _orderService.TotalRevenue();
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPatch("{orderid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int orderid, string status)
        {
            var statuses = await _orderService.UpdateOrderStatus(orderid, status);
            return Ok(statuses);
        }

    }







}

