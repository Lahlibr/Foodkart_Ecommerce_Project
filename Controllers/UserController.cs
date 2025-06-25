using Foodkart.DTOs.ViewDto;
using Foodkart.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodkart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; 

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.AllUser();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("block-unblock/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> BlockAndUnblockUser(int userId)
        {
            try
            {
                var updatedUsers = await _userService.BlockandUnblockUser(userId);
                if (updatedUsers == null )
                {
                    return NotFound(new { message = "User not found or no users available" });
                }
                return Ok(updatedUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
