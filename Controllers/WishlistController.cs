using AutoMapper;
using Foodkart.Service.WishlistService;
using Microsoft.AspNetCore.Mvc;

namespace Foodkart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class WishlistController : ControllerBase
    {
        private readonly IWishlist _wishlistService;
        private readonly ILogger<WishlistController> _logger;
        private readonly IMapper _mapper;
        public WishlistController(IWishlist wishlistService, ILogger<WishlistController> logger, IMapper mapper)
        {
            _wishlistService = wishlistService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpPost("AddToWishlist/{userId}/{productId}")]
        public async Task<IActionResult> AddToWishlist(int userId, int productId)
        {
            try
            {
                var response = await _wishlistService.AddToWishlist(userId, productId);
                if (response.StatusCode == 404)
                {
                    return NotFound(response.Message);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to wishlist");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetWishlist/{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            try
            {
                var response = await _wishlistService.GetWishlist(userId);
                if (response.StatusCode == 404)
                {
                    return NotFound(response.Message);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wishlist");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
