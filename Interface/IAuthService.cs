using Foodkart.Models.DTOs.Auth;
using Foodkart.Models.Entities;
namespace Foodkart.Interface
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegistrationDto regDto);
        Task<string> LoginAsync(LoginDto logDto);
        
    }
}
