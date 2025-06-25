using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;

namespace Foodkart.Service.AddressService
{
    public interface IAddressService
    {
        Task<ApiResponse<AddresViewDto>> AddAddressAsync(int userId, AddresViewDto addressDto);
        Task<ApiResponse<List<AddresViewDto>>> GetAllAddressesAsync(int userId);
        Task<ApiResponse<string>> DeleteAddress(int userid, int addressId);
    }
}
