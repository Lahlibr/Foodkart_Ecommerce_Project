
using System.Security.Claims;
using Foodkart.Data;
using Foodkart.DTOs.ViewDto;
using Foodkart.Service.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodkart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly FoodkartDbContext _context;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
            
        }
        [HttpPost("Add")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddAddress([FromForm] AddresViewDto addressDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid or missing user ID in token.");
            }
            var response = await _addressService.AddAddressAsync(userId, addressDto);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("All")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid or missing user ID in token.");
            }
            var response = await _addressService.GetAllAddressesAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{addressId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid or missing user ID in token.");
            }
            var response = await _addressService.DeleteAddress(userId, addressId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
