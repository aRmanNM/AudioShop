using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AppFileController : ControllerBase
    {
        private readonly IConfigRepository _configRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;
        private readonly IFileService _fileService;

        public AppFileController(IWebHostEnvironment host, 
            IFileService fileService,
            IConfigRepository configRepository,
            IUnitOfWork unitOfWork)
        {
            _host = host;
            _fileService = fileService;
            _configRepository = configRepository;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("app/upload")]
        public async Task<ActionResult> UploadApp(IFormFile file)
        {
            var mobileAppConfig = await _configRepository.GetConfigAsync("LatestMobileAppName");
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Mobile");

            try
            {
                // _fileService.Delete(mobileAppConfig.Value, uploadFolderPath);
                await _fileService.UploadAsync(file, file.FileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            mobileAppConfig.Value = file.FileName;
            _configRepository.SetConfig(mobileAppConfig);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
        
        [HttpGet("app/latest")]
        public async Task<ActionResult<Config>> GetLatestAppVersion()
        {
            var mobileAppConfig = await _configRepository.GetConfigAsync("LatestMobileAppName");
            return Ok(mobileAppConfig);
        }
    }
}
