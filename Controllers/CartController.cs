using System.Reflection;
using System.Security.Claims;
using Amazon.IdentityManagement.Model;
using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;
using Foodkart.Service.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Foodkart.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController:ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly FoodkartDbContext _context;
        private readonly IMapper _mapper;
        

        public CartController(ICartService cartService, FoodkartDbContext context,IMapper mapper)
        {
            _cartService = cartService;
            _context = context;
            _mapper = mapper;
            
        }
        [HttpPost("AddToCart")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToCart(int productid ,int quantity)
        {
            //UserClaimsPrincipal object available in any ASP.NET Core controller (inherited from ControllerBase).It represents the currently authenticated user and holds their identity and all claims issued by the authentication system (like a JWT token).
            //FindFirst(...) searches for a specific claim in the user's token.

             //ClaimTypes.NameIdentifier is a standardized claim type that usually holds the unique identifier of the user(i.e., user ID).

             //In JWT tokens, this maps to the sub(subject) claim, or sometimes a custom claim depending on configuration.

             //If you issued your JWT token with a claim like "sub": "10" or "nameid": "10", this is how you access it.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userid))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var result = await _cartService.AddToCart(userid, productid, quantity);

            return result.StatusCode switch
            {
                200 => Ok(result),
                404 => NotFound(new ApiResponse<string>(404, result.Message)),
                409 => Conflict(new ApiResponse<string>(409, result.Message)),
                _ => BadRequest(new ApiResponse<string>(400, "Bad request"))
            };


        }
        [HttpGet("GetCartItems")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCartItems()
        {
           var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userid) || !int.TryParse(userid, out var userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var result = await _cartService.GetCartItems(userId);
            return result.StatusCode switch
            {
                200 => Ok(result),
                404 => NotFound(new ApiResponse<string>(404, result.Message)),
                _ => BadRequest(new ApiResponse<string>(400, "Bad request"))
            };
        }
        [HttpDelete("RemoveFromCart/{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound(new ApiResponse<string>(404, "Cart not found"));
            }
            var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (itemToRemove == null)
            {
                return NotFound(new ApiResponse<string>(404, "Product not found in cart"));
            }
            cart.CartItems.Remove(itemToRemove);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<string>(200, "Product removed from cart successfully"));
        }
        [HttpPost("AddQuantity")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddQuantity(int productId, int quantity)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var result = await _cartService.AddQuantity(userId, productId, quantity);
            return result.StatusCode switch
            {
                200 => Ok(result),
                404 => NotFound(new ApiResponse<string>(404, result.Message)),
                _ => BadRequest(new ApiResponse<string>(400, "Bad request"))
            };
        }

        [HttpPost("ReduceQuantity")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ReduceQuantity(int productId, int quantity)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var result = await _cartService.ReduceQuantity(userId, productId, quantity);
            return result.StatusCode switch
            {
                200 => Ok(result),
                404 => NotFound(new ApiResponse<string>(404, result.Message)),
                _ => BadRequest(new ApiResponse<string>(400, "Bad request"))
            };
        }
        [HttpPost("ClearCart")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ClearCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var result = await _cartService.ClearCart(userId);
            return result.StatusCode switch
            {
                200 => Ok(result),
                404 => NotFound(new ApiResponse<string>(404, result.Message)),
                _ => BadRequest(new ApiResponse<string>(400, "Bad request"))
            };
        }
        [HttpGet("admin/all-users-carts")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<CartItemViewDto>>>> GetAllUsersCarts()
        {
            try
            {
                var carts = await _context.Carts
                    .Include(c => c.User)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .ToListAsync();

                if (carts == null || !carts.Any())
                    return NotFound(new ApiResponse<string>(404, "No carts found"));

                var userCartsDto = carts.Select(c => new UserDto
                {
                    Id = c.UserId,
                    Username = c.User?.Username,
                    Email = c.User?.Email,
                    Carts = new CartViewDto
                    {
                        Items = c.CartItems.Select(ci => new CartItemViewDto
                        {
                            ProductId = ci.ProductId,
                            ProductName = ci.Product?.ProductName,
                            Image = ci.Product?.ImageUrl,
                            RealPrice = ci.Product?.RealPrice,
                            OfferPrice = ci.Product?.OfferPrice ?? 0,
                            Quantity = ci.Quantity
                        }).ToList()
                    }
                }).ToList();

                return Ok(new ApiResponse<List<UserDto>>(200, "All users' carts retrieved successfully", userCartsDto));
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new ApiResponse<string>(500, "Internal server error"));
            }
        }



    }
}
