using System.Security.Cryptography;
using System.Text;
using Foodkart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Foodkart.DTOs.Auth;
using Foodkart.DTOs.Products;
using Foodkart.Models.Entities.Main;
using AutoMapper;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Carts;


namespace Foodkart.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly FoodkartDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        //For logging messages (info, warnings, errors) using the built-in logging framework.
        private readonly ILogger<AuthService> _logger;

        

        public AuthService(FoodkartDbContext context, IMapper mapper,IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
            _logger = logger;

        }
        public async Task<bool>RegisterAsync(RegistrationDto regDto)
        {
            try
            {
                if(regDto == null)
                {
                    throw new ArgumentNullException(nameof(regDto), "Registration data cannot be null.");
                }
                var IsUserexist = await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == regDto.Email );
                if (IsUserexist != null)
                {
                    _logger.LogWarning($"User with email {regDto.Email} already exists.");
                    return false; // User already exists
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(regDto.Password);
                var user = _mapper.Map<User>(regDto);
                user.PasswordHash = hashedPassword;
                user.Salt = Guid.NewGuid();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true; // Registration successful
            }
            catch(ArgumentNullException ex)
            {
                throw new Exception($"Validation error: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDto> LoginAsync(LoginDto logDto)
        {
            try
            {
                _logger.LogInformation("Attempting to log in user with email: {Email}", logDto.Email);

                var usr = await _context.Users
                    .Include(u => u.Carts)
                        .ThenInclude(c => c.CartItems)
                            .ThenInclude(ci => ci.Product)
                    .SingleOrDefaultAsync(u => u.Email == logDto.Email);

                if (usr == null)
                {
                    _logger.LogWarning("Login failed: User with email {Email} not found.", logDto.Email);
                    return new ResultDto { Error = $"{logDto.Email} is not found" };
                }

                if (usr.Blocked)
                {
                    _logger.LogWarning("Login failed: User with email {Email} is blocked.", logDto.Email);
                    return new ResultDto { Error = "User is blocked" };
                }

                _logger.LogInformation("Validating password for user {Email}", logDto.Email);
                bool isPasswordValid = ValidatePassword(logDto.Password, usr.PasswordHash);

                if (!isPasswordValid)
                {
                    _logger.LogWarning("Login failed: Invalid password for user {Email}.", logDto.Email);
                    return new ResultDto { Error = "Invalid Password" };
                }

                _logger.LogInformation("Password validated. Generating JWT token for user {Email}", logDto.Email);
                var token = GenerateJwtToken(usr);

                // Use null-safe navigation and mapping
                var cartItemDtos = usr.Carts?.CartItems != null
                    ? _mapper.Map<List<CartItemViewDto>>(usr.Carts.CartItems)
                    : new List<CartItemViewDto>();

                return new ResultDto
                {
                    Token = token,
                    Role = usr.Role,
                    Email = usr.Email,
                    Id = usr.Id,
                    Name = usr.Username,
                    Carts = cartItemDtos,
                    Wishlists = "" // Fill later if needed
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {Email}", logDto.Email);
                throw new Exception("An error occurred during login. Please try again later.", ex);
            }
        }



        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       private bool ValidatePassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating password: {ex.Message}");
                throw new Exception("An error occurred while validating the password.", ex);
            }
        }
    }
}
