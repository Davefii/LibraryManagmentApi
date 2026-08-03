using BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusinessLayer.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> SaveImageAsync(
        IFormFile image,
        string folderName)
        {
            if (image == null)
                throw new Exception("Image is required.");

            string[] allowedExtensions = {".jpg",".jpeg",".png",".webp"};

            string extension = Path.GetExtension(image.FileName);

            if(!allowedExtensions.Contains(extension))
                throw new Exception("Invalid image format.");

            const int maxSize = 5 * 1024 * 1024;

            if (image.Length > maxSize)
                throw new Exception("Maximum image size is 5 MB.");

            string fileName = $"{Guid.NewGuid()}{extension}";

            string uploadFolder = Path.Combine(_environment.WebRootPath,"uploads",folderName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string filePath = Path.Combine(uploadFolder,fileName);

            using var stream = new FileStream(filePath,FileMode.Create);

            await image.CopyToAsync(stream);

            return $"uploads/{folderName}/{fileName}";
        }
        public void DeleteImage(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            string fullPath =
                Path.Combine(
                    _environment.WebRootPath,
                    imagePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
