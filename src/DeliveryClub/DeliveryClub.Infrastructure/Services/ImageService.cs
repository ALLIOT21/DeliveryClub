using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DeliveryClub.Infrastructure.Services
{
    public class ImageService
    {
        private readonly string _webRootPath;

        public ImageService(string webRootPath)
        {
            _webRootPath = webRootPath;
        }

        public async Task<string> CreateImage(IFormFile image)
        {
            string uniqueFileName = null;            
            if (image != null)
            {
                var folderPath = Path.Combine(_webRootPath, "resources\\img\\");
                uniqueFileName = CreateUniqueFileName(image.FileName);
                string filePath = CreateFilePath(uniqueFileName, folderPath);
                await image.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }
            return uniqueFileName;
        }
        private string CreateUniqueFileName(string fileName)
        {
            return Guid.NewGuid().ToString() + "_" + fileName;
        }

        private string CreateFilePath(string fileName, string fileFolder)
        {
            return Path.Combine(fileFolder, fileName);
        }        

        public void DeleteImage(string imageName)
        {
            if (imageName != null)
            {
                var folderPath = Path.Combine(_webRootPath, "resources\\img\\");
                var imagePath = Path.Combine(folderPath, imageName);
                File.Delete(imagePath);
            }
        }
        
}
}
