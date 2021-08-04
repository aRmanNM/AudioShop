using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Models.Landing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LandingController : ControllerBase
    {
        private readonly ILandingRepository _landingRepository;
        private readonly IWebHostEnvironment _host;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public LandingController(ILandingRepository landingRepository,
            IWebHostEnvironment host,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _host = host;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _landingRepository = landingRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Landing>>> GetLandings()
        {
            var landings = await _landingRepository.GetLandings();
            return Ok(landings);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Landing>> CreateLanding(Landing landing)
        {
            await _landingRepository.CreateLanding(landing);
            await _unitOfWork.CompleteAsync();
            return Ok(landing);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Landing>> EditLanding(Landing landing)
        {
            _landingRepository.EditLanding(landing);
            await _unitOfWork.CompleteAsync();
            return Ok(landing);
        }

        [HttpPost("{landingId}/phonenumber")]
        public async Task<ActionResult<LandingPhoneNumber>> CreateLandingPhoneNumber(int landingId, [FromBody] LandingPhoneNumber landingPhoneNumber)
        {
            await _landingRepository.CreateLandingPhoneNumber(landingPhoneNumber);
            await _unitOfWork.CompleteAsync();
            return Ok(landingPhoneNumber);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{landingId}/files/{field}")]
        public async Task<ActionResult<Object>> UploadFile(int landingId, string field, IFormFile file)
        {
            var landing = await _landingRepository.GetLandingById(landingId);
            if (landing == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Landings", landingId.ToString());
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            try
            {
                await _fileService.UploadAsync(file, fileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (field.ToLower() == "logo")
            {
                var logo = new Photo
                {
                    FileName = fileName
                };

                landing.Logo = logo;
            }
            else if (field.ToLower() == "media")
            {
                var media = new ContentFile
                {
                    FileName = fileName
                };

                landing.Media = media;
            }

            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        // TODO: method to export phonenumbers
    }
}