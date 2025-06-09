using System.Diagnostics.Metrics;
using System.IO;
using CloudinaryDotNet;

namespace Foodkart.Service.CloudinaryService
{
    public class CloudinaryService:ICloudinaryService
    {
        private readonly CloudinaryService _cloudinaryService;
        public CloudinaryService(IConfiguration configuration)
        {
            // Takes data from the cloud by the name and  etc
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];
            // create a Cloudinary account using the cloud name, api key and api secret
            var account = new Account(cloudName, apiKey, apiSecret);
            // initialize the Cloudinary service with the account
            _cloudinaryService = new Cloudinary(account);
            
        }
        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;
            //OpenReadStream(): Accesses the file data as a stream.
            // The using statement ensures that the stream is disposed of after use, preventing memory leaks.
            using (var stream = file.OpenReadStream())
            //Upload Configuration
            { var uploadParams = new ImageUploadParams {File  = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill") // Resize the image to 500x500 pixels
            };
                // Upload the image to Cloudinary
                var uploadResult = await _cloudinaryService.UploadAsync(uploadParams);
                // Check if the upload was successful and return the URL of the uploaded image
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUri.ToString();
                }
                else
                {
                    throw new Exception("Image upload failed: " + uploadResult.Error?.Message);
                }
                ;
              
            }
            }
        
    }
}
