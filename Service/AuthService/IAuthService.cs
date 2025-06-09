using Foodkart.DTOs.Auth;
using Foodkart.Models.Entities;
namespace Foodkart.Service.AuthService
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegistrationDto regDto);
        Task<ResultDto> LoginAsync(LoginDto logDto);
        
    }
}
