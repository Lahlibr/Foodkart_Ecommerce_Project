using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.ViewDto;
using Microsoft.EntityFrameworkCore;
using System;

namespace Foodkart.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly FoodkartDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        public UserService(FoodkartDbContext context, ILogger<UserService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<List<UserDto>> AllUser()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Carts)
                        .ThenInclude(c => c.CartItems)
                            .ThenInclude(ci => ci.Product)
                    .Include(u => u.Wishlists)
                    .ToListAsync();

                return _mapper.Map<List<UserDto>>(users);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserDto> BlockandUnblockUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null;
            if (user.Role == "Admin") //  Prevent blocking Admins
                throw new InvalidOperationException("Admins cannot be blocked");
            user.Blocked = !user.Blocked;
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }


    }
}
