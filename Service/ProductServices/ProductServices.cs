using System;
using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.AddDto;
using Foodkart.DTOs.Products;
using Foodkart.Models.Entities.Products;
using Foodkart.Service.CloudinaryService;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Service.ProductServices
{
    public class ProductServices : IProductService
    {
        private readonly FoodkartDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        public ProductServices(FoodkartDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<List<ProductViewDto>> GetAllProducts()
        {
            try
            {
                var products = await _context.Products
                    // Eagerly loads related category data for each product. 
                    .Include(x => x.category)
                    .ToListAsync();



                return _mapper.Map<List<ProductViewDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products: " + ex.Message, ex);
            }
        }
        public async Task<ProductViewDto> GetProductsById(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(x => x.category)
                    .FirstOrDefaultAsync(x => x.ProductId == id);
                if (product == null) return null;
                return _mapper.Map<ProductViewDto>(product);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving product by ID: " + ex.Message, ex);
            }
        }
        public async Task<List<ProductViewDto>> GetProductByCategory(string category)
        {
            try
            {
                var products = await _context.Products
                    .Include(x => x.category)
                    .Where(x => x.category.Name == category)
                    .ToListAsync();
                if (products.Count == 0) return null;
                return _mapper.Map<List<ProductViewDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products by category: " + ex.Message, ex);
            }
        }
        //Creating a product 
        public async Task<Product> CreateProduct(ProductDto pdDto, IFormFile image)
        {
            try
            {
                if (pdDto == null) throw new ArgumentNullException(nameof(pdDto), "Product data cannot be null.");
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == pdDto.CategoryId);
                if (category == null) throw new ArgumentException("Invalid category ID.", nameof(pdDto.CategoryId));
                if (image == null)
                {
                    throw new ArgumentNullException(nameof(image), "Image cannot be null.");
                }
                string imageUrl = await _cloudinaryService.UploadImage(image);
                var product = _mapper.Map<Product>(pdDto);
                product.ImageUrl = imageUrl;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception($"Validation error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating product: " + ex.Message, ex);
            }
        }
        //Updating a product
        public async Task<bool> UpdateProduct(int id, ProductDto editpdDto, IFormFile image)
        {
            //Tries to find the existing product in the database with the given ID.
            var productex = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            var categoryex = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == editpdDto.CategoryId);
            if (productex == null || categoryex == null)
            {
                throw new ArgumentException("Invalid product ID or category ID.");
            }
            try
            {
                // Maps the properties from the DTO to the existing product entity.
                _mapper.Map(editpdDto, productex);
                productex.CategoryId = editpdDto.CategoryId;
                if (image != null && image.Length > 0)
                {
                    productex.ImageUrl = await _cloudinaryService.UploadImage(image);
                }
                // Saves the changes to the database.
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product: " + ex.Message, ex);
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            try
            {
                if (product == null) throw new ArgumentException("Invalid product ID.", nameof(id));
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ArgumentException ex) // Catch specific ArgumentException if you want to differentiate
            {
                throw new Exception($"Validation error: {ex.Message}", ex); // Re-throw with more context
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting product: " + ex.Message, ex);
            }
        }
        public async Task<List<ProductViewDto>> SearchProduct(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new List<ProductViewDto>();
                }
                var lowerSearchTerm = searchTerm.Trim().ToLower();
                var products = await _context.Products
                    .Include(x => x.category)
                    .Where(x => x.ProductName.ToLower().Contains(lowerSearchTerm) ||
                                x.Description.ToLower().Contains(lowerSearchTerm))
                    .ToListAsync();
                return _mapper.Map<List<ProductViewDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching products: " + ex.Message, ex);
            }


        }
    }
}

