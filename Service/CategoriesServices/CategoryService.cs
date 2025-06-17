using AutoMapper;
using Foodkart.Data;
using Foodkart.DTOs.AddDto;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Main;
using Foodkart.Service.CloudinaryService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Service.CategoriesServices
{
    public class CategoryService : ICategoryService
    {
        private readonly FoodkartDbContext _context;
        private readonly IMapper _mapper;
        
        private readonly ICloudinaryService _cloudinaryService;

        public CategoryService(FoodkartDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;

        }

        public async Task<bool> AddCategory(CreateCategoryDto ctDto, IFormFile image)
        {
            try
            {
                if (ctDto == null)
                    throw new ArgumentNullException(nameof(ctDto), "Category data cannot be null.");

                // Check for name conflict
                var isExist = await _context.Categories
                    .AnyAsync(x => x.Name.ToLower() == ctDto.Name.ToLower());

                if (isExist)
                    throw new Exception("Category with this name already exists.");

                // Upload image
                if (image == null || image.Length == 0)
                    throw new ArgumentNullException(nameof(image), "Image cannot be null.");

                string imagePath = await _cloudinaryService.UploadImage(image);

                // Map and set image URL
                var category = _mapper.Map<Category>(ctDto);
                category.ImageUrl = imagePath;

                // Save to DB
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category: " + ex.Message, ex);
            }
        }






        public async Task<List<CategoryViewDto>> GetAllCategories()
        {
            var categories = await _context.Categories
                .Include(c=>c.Products)
                .ToListAsync();
            if (categories == null || categories.Count == 0)
            {
                throw new Exception("No categories found.");
            }
            return _mapper.Map<List<CategoryViewDto>>(categories);
        }

        public async Task<CategoryViewDto> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }
            return _mapper.Map<CategoryViewDto>(category);
        }

        public async Task<bool> UpdateCategory(int id, CategoryViewDto categoryDto, IFormFile image)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new Exception("Category not found.");

            // Check for name conflict
            var isExist = await _context.Categories
                .AnyAsync(x => x.Name.ToLower() == categoryDto.Name.ToLower() && x.CategoryId != id);
            if (isExist)
                throw new Exception("Category with this name already exists.");

            // Update properties
            _mapper.Map(categoryDto, category);

            // If new image is provided, update it
            if (categoryDto.ImageUrl != null && categoryDto.ImageUrl.Length > 0)
            {
                string imagePath = await _cloudinaryService.UploadImage(image);
                category.ImageUrl = imagePath;
            }

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
