using Foodkart.DTOs.AddDto;
using Foodkart.DTOs.Products;
using Foodkart.Models.Entities.Products;

namespace Foodkart.Service.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductViewDto>> GetAllProducts();
        Task<ProductViewDto> GetProductsById(int id);
        Task<List<ProductViewDto>> GetProductByCategory(string category);
        Task<Product> CreateProduct(ProductDto pddto, IFormFile image);
        Task<bool> UpdateProduct(int id, ProductDto pddto, IFormFile image);
        Task<bool> DeleteProduct(int id);
        Task<List<ProductViewDto>> SearchProduct(string search);

    }
}
