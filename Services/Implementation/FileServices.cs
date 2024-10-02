using FoodDonationWebApp.Services.Interfaces;
using System.Drawing;

namespace FoodDonationWebApp.Services.Implementation
{
    public class FileServices : IFileServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileServices(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is invalid.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Only images (.jpg, .jpeg, .png) are allowed.");
            }

            if (file.Length > 2 * 1024 * 1024)
            {
                throw new Exception("File size must be less than 2 MB.");
            }

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }

    }
}
