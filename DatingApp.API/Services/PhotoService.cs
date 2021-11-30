using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using DatingApp.API.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            this.cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    };
                    uploadResult = await this.cloudinary.UploadAsync(uploadParams);
                }
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var delParams = new DeletionParams(publicId);
            var result = await this.cloudinary.DestroyAsync(delParams);
            return result;
        }
    }
}