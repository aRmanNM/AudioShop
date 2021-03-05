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
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;
        private readonly IFileService _fileService;
        private readonly AudioOptions _audioOptions;

        public FilesController(IEpisodeRepository episodeRepository,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment host,
            IOptions<AudioOptions> options,
            IFileService fileService)
        {
            _episodeRepository = episodeRepository;
            _unitOfWork = unitOfWork;
            _host = host;
            _fileService = fileService;
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
            if (file.Length > _audioOptions.MaxBytes) return BadRequest("حجم فایل بیش از حد بزرگ است");
            if (!_audioOptions.IsSupported(file.FileName)) return BadRequest("فرمت فایل درست نیست");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Files", episode.CourseId.ToString());
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            try
            {
                await _fileService.Upload(file, fileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var duration = int.Parse(file.FileName.Split('_')[1]);
            var audio = new Audio()
            {
                FileName = fileName,
                Duration = duration
            };

            episode.TotalAudiosDuration = episode.TotalAudiosDuration + duration;
            episode.Audios.Add(audio);
            await _unitOfWork.CompleteAsync();
            return Ok(audio);
        }

        [Authorize(Roles="Admin")]
        [HttpDelete("episodes/{episodeId}/audios")]
        public async Task<ActionResult> DeleteEpisodeAudios(int episodeId)
        {
            var episode = await _episodeRepository.GetEpisodeById(episodeId);
            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Files", episode.CourseId.ToString());
            if (episode == null) return NotFound();
            try
            {
                foreach (var item in episode.Audios)
                {
                    _fileService.Delete(item.FileName, uploadFolderPath);
                }
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }

            episode.Audios = null;
            episode.TotalAudiosDuration = 0;
            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
