using Microsoft.AspNetCore.Http;
using NewBlog.Entity.DTOs.Images;
using NewBlog.Entity.Enums;

namespace NewBlog.Service.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadedDto> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null);
        void Delete(string imageName);
    }
}
