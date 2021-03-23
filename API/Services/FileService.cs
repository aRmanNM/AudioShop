using System.IO;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
    public class FileService : IFileService
    {
        public void Delete(string fileName, string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);
            File.Delete(filePath);
        }

        public async Task UploadAsync(IFormFile file, string fileName, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}