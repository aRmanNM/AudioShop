using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IFileService
    {
        Task UploadAsync(IFormFile file, string fileName, string folderPath);
        void Delete(string fileName, string folderPath);
    }
}