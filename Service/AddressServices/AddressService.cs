using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Service.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly FoodkartDbContext _context;
        private readonly IMapper _mapper;
        
        public AddressService(FoodkartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse<string>> AddAddressAsync(int userId, AddresViewDto addressDto)
        {
            try
            {
                var address = _mapper.Map<Address>(addressDto);
                address.UserId = userId;
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(200, "Address added successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, $"An error occurred while adding the address: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AddresViewDto>>> GetAllAddressesAsync(int userId)
        {
            try
            {
                var addresses = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();
                if (addresses == null || !addresses.Any())
                {
                    return new ApiResponse<List<AddresViewDto>>(404, "No addresses found for this user.");
                }
                var addressDtos = _mapper.Map<List<AddresViewDto>>(addresses);
                return new ApiResponse<List<AddresViewDto>>(200, "Addresses retrieved successfully.", addressDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AddresViewDto>>(500, $"An error occurred while retrieving addresses: {ex.Message}");
            }
        }
        public async Task<ApiResponse<string>> DeleteAddress(int userid, int addressId)
        {
            try
            {
                var address = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == userid && a.AddressId == addressId);

                if (address == null)
                {
                    return new ApiResponse<string>(404, "Address not found.");
                }
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>(200, "Address deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, $"An error occurred while deleting the address: {ex.Message}");
            }
        }



    }
}