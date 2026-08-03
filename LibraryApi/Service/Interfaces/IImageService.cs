using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    internal interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile image, string folderName);

        void DeleteImage(string? imagePath);
    }
}
