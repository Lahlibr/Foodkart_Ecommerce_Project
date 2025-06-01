using Foodkart.Models.DTOs.Auth;
using Foodkart.Service;
using Foodkart.Models.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Foodkart.Interface;


namespace Foodkart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authController;
        public AuthController(IAuthService authController)
        {
            _authController = authController;
        }
        [HttpPost("register")]
        //"Hey, take the JSON/XML data from the HTTP request body and turn it into a C# object for me."
        public async Task<IActionResult> Register([FromBody] RegistrationDto regDto)
        {
            var result = await _authController.RegisterAsync(regDto);
            if (result == null)
                return BadRequest("Registration failed. Email might already be in use.");
            return Ok("Registration successful. Please verify your email to complete the process.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto logDto)
        {
            var result = await _authController.LoginAsync(logDto);
            if (result == null)
                return Unauthorized("Invalid email or password.");
            return Ok(result);
        }
    }
    
}
