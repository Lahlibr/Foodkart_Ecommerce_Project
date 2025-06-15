using Foodkart.Service;
using Foodkart.Models.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Foodkart.DTOs.Auth;
using Foodkart.Service.AuthService;


namespace Foodkart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authController)
        {
            _authService = authController;
        }
        [HttpPost("register")]
        //"Hey, take the JSON/XML data from the HTTP request body and turn it into a C# object for me."
        public async Task<IActionResult> Register([FromBody] RegistrationDto regDto)
        {
            var result = await _authService.RegisterAsync(regDto);
            if (!result)
                return BadRequest(new
                {
                    success = false,
                    message = "Registration failed",
                    errors = new { email = "Email might already be in use" }
                });
            return Ok(new
            {
                success = true,
                message = "Registration successful. Please verify your email"
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto logDto)
        {
            var result = await _authService.LoginAsync(logDto);
            if (!string.IsNullOrEmpty(result.Error))
            {
                if (result.Error.Contains("blocked"))
                    return StatusCode(403, new { message = result.Error });

                return Unauthorized(new { message = result.Error });
            }
            return Ok(new {result});
        }
        
    }
    
}
