using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Models.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;
        private readonly AudioOptions _audioOptions;

        public FilesController(IEpisodeRepository episodeRepository,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment host,
            IOptions<AudioOptions> options)
        {
            _episodeRepository = episodeRepository;
            _config = config;
            _unitOfWork = unitOfWork;
            _host = host;
            _audioOptions = options.Value;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("episodes/{episodeId}/audios")]
        public async Task<ActionResult<Audio>> UploadAudioFile(int episodeId, IFormFile file)
        {
            var episode = await _episodeRepository.GetEpisodeById(episodeId);
            if (episode == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");
            if (file.Length > _audioOptions.MaxBytes) return BadRequest("max file size exceeded");
            if (!_audioOptions.IsSupported(file.FileName)) return BadRequest("format not valid");

            var uploadFolderPath = Path.Combine(_host.ContentRootPath, "Files", "Courses", episode.CourseId.ToString());
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

            var audio = new Audio()
            {
                FileName = fileName
            };

            episode.Audios.Add(audio);
            await _unitOfWork.CompleteAsync();
            return Ok(audio);
        }

        [HttpGet("episodes/{episodeId}/audios/{audioId}")]
        public async Task<ActionResult> GetAudioFile(int episodeId, int audioId)
        {
            var episode = await _episodeRepository.GetEpisodeById(episodeId);
            if (episode == null) return NotFound();

            var fileName = episode.Audios.FirstOrDefault(a => a.Id == audioId)?.FileName;
            if (fileName == null) { return NotFound("file not found"); }

            var fileExtension = Path.GetExtension(fileName);
            var mimeType = fileExtension.ToLower() switch
            {
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                _ => null
            };

            var filePath = Path.Combine(_host.ContentRootPath, "Files", "Courses", episode.CourseId.ToString(), fileName ?? string.Empty);
            return PhysicalFile(filePath, mimeType);
        }
    }
}
