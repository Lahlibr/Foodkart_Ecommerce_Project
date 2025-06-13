using Foodkart.DTOs.ViewDto;
using Foodkart.Service.CategoriesServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodkart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("Byid{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromForm] CategoryViewDto categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Category data is missing.");

            try
            {
                var image = Request.Form.Files.FirstOrDefault();
                if (image == null || image.Length == 0)
                    return BadRequest("Image file is required.");

                var result = await _categoryService.AddCategory(categoryDto, image); 
                return result ? Ok("Category added successfully.") : BadRequest("Failed to add category.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryViewDto categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Category data is missing.");

            try
            {
                var image = Request.Form.Files.FirstOrDefault(); 
                var result = await _categoryService.UpdateCategory(id, categoryDto, image); 
                return result ? Ok("Category updated successfully.") : BadRequest("Failed to update category.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(id);
                if (result)
                {
                    return Ok("Category deleted successfully.");
                }
                else
                {
                    return BadRequest("Failed to delete category.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
