using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Interface
{
    public interface IPhotoService
    {
        //IFormFile is used to handle files in the httprequests
         Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
         Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}