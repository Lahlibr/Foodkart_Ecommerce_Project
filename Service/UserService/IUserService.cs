using Foodkart.DTOs.ViewDto;

namespace Foodkart.Service.UserService
{
    public interface IUserService
    {
        Task<List<UserDto>> AllUser();
        Task<UserDto> BlockandUnblockUser(int userId);
    }
}