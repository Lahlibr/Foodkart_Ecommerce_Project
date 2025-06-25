using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Carts;
using Foodkart.Models.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Service.CartService
{
    public class CartService : ICartService
    {
        private readonly FoodkartDbContext _context;
        private readonly ILogger<CartService> _logger;
        private readonly IMapper _mapper;
        public CartService(FoodkartDbContext context, ILogger<CartService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartViewDto>> AddToCart(int userId, int productId, int quantity)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Carts)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return new ApiResponse<CartViewDto>(404, "User not found", null);

                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (product == null)
                    return new ApiResponse<CartViewDto>(404, "Product not found", null);

                // Check available stock
                var currentCartQuantity = user.Carts?.CartItems
                    .Where(ci => ci.ProductId == productId)
                    .Sum(ci => ci.Quantity) ?? 0;

                var requestedTotal = currentCartQuantity + quantity;

                if (product.InStock < requestedTotal)
                {
                    return new ApiResponse<CartViewDto>(400,
                        $"Only {product.InStock - currentCartQuantity} items available in stock",
                        null);
                }

                // Rest of your existing code...
                var cart = user.Carts ?? await CreateCartForUserAsync(user);
                var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.TotalPrice += product.OfferPrice * quantity;
                }
                else
                {
                    var cartItem = new CartItems
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        TotalPrice = product.OfferPrice * quantity
                    };
                    cart.CartItems.Add(cartItem);
                }

                await _context.SaveChangesAsync();
                return new ApiResponse<CartViewDto>(200, "Product added to cart successfully", _mapper.Map<CartViewDto>(cart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to cart");
                return new ApiResponse<CartViewDto>(500, "Internal server error", null);
            }
        }
        private async Task<Cart> CreateCartForUserAsync(User user)
        {
            var cart = new Cart
            {
                UserId = user.Id,
                CartItems = new List<CartItems>()
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<ApiResponse<CartViewDto>> GetCartItems(int userId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
                //.Any() — This is a LINQ method that returns true if the collection contains at least one item
                if (cart == null || !cart.CartItems.Any())
                {
                    return new ApiResponse<CartViewDto>(404, "Cart is empty or not found", null);
                }
                var cartViewDto = _mapper.Map<CartViewDto>(cart);
                return new ApiResponse<CartViewDto>(200, "Cart items retrieved successfully", cartViewDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart items");
                return new ApiResponse<CartViewDto>(500, "Internal server error", null);
            }
        }

        public async Task<ApiResponse<CartViewDto>> RemoveFromCart(int userId, int productId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    return new ApiResponse<CartViewDto>(404, "Cart is empty or not found", null);
                }
                var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (itemToRemove == null)
                {
                    return new ApiResponse<CartViewDto>(404, "Item not found in cart", null);
                }
                cart.CartItems.Remove(itemToRemove);
                await _context.SaveChangesAsync();
                return new ApiResponse<CartViewDto>(200, "Item removed from cart successfully", _mapper.Map<CartViewDto>(cart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return new ApiResponse<CartViewDto>(500, "Internal server error", null);
            }
        }
        public async Task<ApiResponse<CartViewDto>> AddQuantity(int userId, int productId, int quantity)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.CartItems.Any())
                {
                    return new ApiResponse<CartViewDto>(404, "Cart is empty or not found", null);
                }

                var itemToUpdate = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (itemToUpdate == null)
                {
                    return new ApiResponse<CartViewDto>(404, "Item not found in cart", null);
                }

                // Check stock before adding quantity
                var newTotalQuantity = itemToUpdate.Quantity + quantity;
                if (itemToUpdate.Product.InStock < newTotalQuantity)
                {
                    return new ApiResponse<CartViewDto>(400,
                        $"Only {itemToUpdate.Product.InStock} items available in stock",
                        null);
                }

                itemToUpdate.Quantity = newTotalQuantity;
                itemToUpdate.TotalPrice = itemToUpdate.Product.OfferPrice * newTotalQuantity;

                await _context.SaveChangesAsync();

                return new ApiResponse<CartViewDto>(200, "Item quantity updated successfully", _mapper.Map<CartViewDto>(cart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantity in cart");
                return new ApiResponse<CartViewDto>(500, "Internal server error", null);
            }
        }
        public async Task<ApiResponse<CartViewDto>> ReduceQuantity(int userId, int productId, int quantity)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    return new ApiResponse<CartViewDto>(404, "Cart is empty or not found", null);
                }
                var itemToDecrease = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (itemToDecrease == null)
                {
                    return new ApiResponse<CartViewDto>(404, "Item not found in cart", null);
                }
                itemToDecrease.Quantity -= quantity;
                if (itemToDecrease.Quantity <= 0)
                {
                    cart.CartItems.Remove(itemToDecrease);
                }
                else
                {
                    itemToDecrease.TotalPrice -= itemToDecrease.Product.OfferPrice * quantity;
                }
                await _context.SaveChangesAsync();
                return new ApiResponse<CartViewDto>(200, "Item removed from cart successfully", _mapper.Map<CartViewDto>(cart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return new ApiResponse<CartViewDto>(500, "Internal server error", null);
            }
        }

        public async Task<ApiResponse<CartViewDto>> ClearCart(int userId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    return new ApiResponse<CartViewDto>(404, "Cart is empty or not found", null);
                }
                cart.CartItems.Clear();
                await _context.SaveChangesAsync();
                return new ApiResponse<CartViewDto>(200, "Cart cleared successfully", _mapper.Map<CartViewDto>(cart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return new ApiResponse<CartViewDto>(500, "Internal server error", null);
            }
        }
        public async Task<ApiResponse<List<CartViewDto>>> AllUsersCart(int productId)
        {
            try
            {
                var carts = await _context.Carts
                    .Include(c => c.User)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .Where(c => c.CartItems.Any(ci => ci.ProductId == productId))
                    .ToListAsync();

                if (carts == null || carts.Count == 0)
                {
                    return new ApiResponse<List<CartViewDto>>(404, "No carts found for this product", null);
                }

                // Map to list of CartViewDto
                var allUsersCarts = _mapper.Map<List<CartViewDto>>(carts);

                return new ApiResponse<List<CartViewDto>>(200, "All users cart retrieved successfully", allUsersCarts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users' carts");
                return new ApiResponse<List<CartViewDto>>(500, "Internal server error", null);
            }
        }

    }
}
