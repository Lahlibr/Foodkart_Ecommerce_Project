using System.Security.Cryptography;
using System.Text;
using Foodkart.Data;
using Foodkart.Interface;
using Foodkart.Models.DTOs.Auth;
using Foodkart.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Service
{
    public class AuthService : IAuthService
    {
        private readonly FoodkartDbContext _context;
        public AuthService(FoodkartDbContext context)
        {
            _context = context;
        }
        public async Task<bool> RegisterAsync(RegistrationDto regDto)
        {
            if (await _context.Registration.AnyAsync(u => u.Email == regDto.Email))
                return false;
            var salt = Guid.NewGuid();
            var passwordHash = HashPassword(regDto.Password, salt);
            var user = new Registration
            {
                Username = regDto.Username,
                Email = regDto.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                EmailVerified = false,
                Blocked = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
            };
            _context.Registration.Add(user);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<string> LoginAsync(LoginDto logDto)
        {
            var user = await _context.Registration
                .FirstOrDefaultAsync(u => u.Email == logDto.Email && !u.Blocked);
            if (user == null || user.Blocked) return null;
            var hash = HashPassword(logDto.Password, user.Salt);
            if (hash != user.PasswordHash) return null;
            return "Login successful"; // In a real application, you would return a JWT or session token here.
        }
        private string HashPassword(string password, Guid salt)
        {   //SHA256.Create(): Initializes the hashing algorithm.
            using var sha256 = SHA256.Create();
            //password + salt: Combines inputs as a UTF8 byte array.
            var combined = Encoding.UTF8.GetBytes(password + salt);
            //ComputeHash: Generates a 256-bit hash.
            var hash = sha256.ComputeHash(combined);
            //ToBase64String: Converts the hash to a storable string.
            return Convert.ToBase64String(hash);
        }
    }
}
