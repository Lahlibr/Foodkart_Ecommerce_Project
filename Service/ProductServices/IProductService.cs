using Foodkart.DTOs.Products;

namespace Foodkart.Service.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductViewDto>> GetAllProducts();
        Task<ProductViewDto> GetProductsById(int id);
        Task<List<ProductViewDto>> GetProductByCategory(string category);

    }
}
