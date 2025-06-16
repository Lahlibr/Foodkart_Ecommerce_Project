using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;
using Foodkart.Service.WishlistService;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Service.WishlistService
{
    public class WishlistService : IWishlist
    {
        private readonly FoodkartDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<WishlistService> _logger;
        public WishlistService(FoodkartDbContext context, IMapper mapper, ILogger<WishlistService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponse<WishlistViewDto>> AddToWishlist(int userId, int productId)
        {
            try
            {
                // Check if user exists
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<WishlistViewDto>(404, "User not found", null);
                }

                // Check if product exists
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return new ApiResponse<WishlistViewDto>(404, "Product not found", null);
                }

                // Check for duplicate wishlist entry
                var existingWishlist = await _context.Wishlists
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

                if (existingWishlist != null)
                {
                    // If already exists, remove it (toggle off)
                    _context.Wishlists.Remove(existingWishlist);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<WishlistViewDto>(200, "Item removed from wishlist", null);
                }
                else
                {
                    // Otherwise, add it (toggle on)
                    var newWishlist = new Wishlist
                    {
                        UserId = userId,
                        ProductId = productId
                    };

                    _context.Wishlists.Add(newWishlist);
                    await _context.SaveChangesAsync();

                    // Optionally map to WishlistViewDto
                    var wishlistDto = new WishlistViewDto
                    {
                        WishlistId = newWishlist.WishlistId,
                        UserId = userId,
                        UserName = user.Username,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Image = product.ImageUrl,
                        RealPrice = product.RealPrice,
                        OfferPrice = product.OfferPrice
                    };

                    return new ApiResponse<WishlistViewDto>(200, "Item added to wishlist successfully", wishlistDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to wishlist");
                return new ApiResponse<WishlistViewDto>(500, "An error occurred while adding item to wishlist", null);
            }
        }

        public async Task<ApiResponse<List<WishlistViewDto>>> GetWishlist(int userId)
        {
            try
            {
                // Check if user exists
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<List<WishlistViewDto>>(404, "User not found", null);
                }
                // Retrieve wishlist items for the user
                var wishlists = await _context.Wishlists
                    .Where(w => w.UserId == userId)
                    .Include(w => w.products)
                    .ToListAsync();
                if (wishlists.Count == 0)
                {
                    return new ApiResponse<List<WishlistViewDto>>(404, "No items in wishlist", null);
                }
                // Map to WishlistViewDto
                var wishlistDtos = _mapper.Map<List<WishlistViewDto>>(wishlists);
                return new ApiResponse<List<WishlistViewDto>>(200, "Wishlist retrieved successfully", wishlistDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wishlist");
                return new ApiResponse<List<WishlistViewDto>>(500, "An error occurred while retrieving wishlist", null);
            }
        }

    }
}
