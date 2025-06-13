using Amazon.IdentityManagement.Model;
using CloudinaryDotNet.Actions;
using Foodkart.DTOs.AddDto;
using Foodkart.Service.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StackExchange.Redis;

namespace Foodkart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }
        [HttpGet("PdbyId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductsById(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }
        [HttpGet("pdbyct/{category}")]
        [Authorize]
        public async Task<IActionResult> GetProductByCategory(string category)
        {
            var products = await _productService.GetProductByCategory(category);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found in category {category}.");
            }
            return Ok(products);
        }
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto, IFormFile image)
        {
            if (productDto == null || image == null)
            {
                return BadRequest("Product data or image is missing.");
            }
            var createdProduct = await _productService.CreateProduct(productDto, image);
            if (createdProduct == null)
            {
                return BadRequest("Failed to create product.");
            }
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
        }
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto productDto, IFormFile image)
        {
            if (productDto == null || image == null)
            {
                return BadRequest("Product data or image is missing.");
            }
            var updatedProduct = await _productService.UpdateProduct(id, productDto, image);
            if (updatedProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(updatedProduct);
        }
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _productService.DeleteProduct(id);
            if (!isDeleted)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return NoContent(); // 204 No Content
        }
        [HttpGet("Search/{name}")]
        [Authorize]
        public async Task<IActionResult> SearchProducts(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term cannot be empty.");
            }
            var products = await _productService.SearchProduct(name);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found matching '{name}'.");
            }
            return Ok(products);
        }
    }
}
