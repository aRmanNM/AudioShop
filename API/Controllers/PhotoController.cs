using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Models.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly PhotoOptions _photoOptions;

        public PhotoController(IWebHostEnvironment host,
            ICourseRepository courseRepository,
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IOptions<PhotoOptions> options)
        {
            _host = host;
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
            _config = config;
            _photoOptions = options.Value;
        }

        [Authorize(Roles="Admin")]
        [HttpPost("course/{courseId}/photo")]
        public async Task<ActionResult<Photo>> Upload(int courseId, IFormFile file)
        {
            var course = await _courseRepository.GetCourseById(courseId);
            if (course == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");
            if (file.Length > _photoOptions.MaxBytes) return BadRequest("max file size exceeded");
            if (!_photoOptions.IsSupported(file.FileName)) return BadRequest("format not valid");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Files", course.Id.ToString());
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo()
            {
                FileName = fileName
            };

            course.Photo = photo;
            await _unitOfWork.CompleteAsync();
            return Ok(photo);
        }
    }
}
