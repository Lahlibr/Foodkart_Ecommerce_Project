namespace Foodkart.Service.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage(IFormFile file);
    }
}
