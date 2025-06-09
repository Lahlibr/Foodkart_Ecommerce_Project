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
        public ProductServices(FoodkartDbContext context, IMapper mapper,ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<List<ProductDto>> GetAllProducts(List<ProductDto> productsall)
        {
            try
            {
                var products = await _context.Products
                    // Eagerly loads related category data for each product. 
                    .Include(x => x.category)
                    .ToListAsync();
                if (products.Count != 0) { 
                    var Productsall = products.Select(x=> new ProductViewDto
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Offer_Price = x.Offer_Price,
                        Real_Price = x.Real_Price,
                        InStock = x.InStock,
                        Type = x.Type,
                        ImageUrl = x.ImageUrl
                    }).ToList();                    return productsall;
                }


                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products: " + ex.Message, ex);
            }
        }
        public async Task<ProductViewDto> GetByIdAsync(int id)
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
        public async Task<List<ProductViewDto>> GetByCategoryAsync(string category)
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
        public async Task<Product> CreateAsync(ProductDto pdDto,IFormFile image)
        {
            try
            {
                if (pdDto == null) throw new ArgumentNullException(nameof(pdDto), "Product data cannot be null.");
                var category = await _context.Categories.FirstOrDefaultAsync(x=>x.CategoryId==pdDto.CategoryId);
                if (category == null) throw new ArgumentException("Invalid category ID.", nameof(pdDto.CategoryId));
                if (image != null)
                {
                    throw new ArgumentNullException(nameof(image), "Image cannot be null.");
                }
                _context.Products.Add(product);
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
        //Updating a product
        public async Task<bool> UpdateAsync(int id, ProductDto pdDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            _mapper.Map(pdDto, product);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
