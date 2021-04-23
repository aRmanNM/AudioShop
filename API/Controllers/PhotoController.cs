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
using System.Security.Claims;

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
        private readonly IFileService _fileService;
        private readonly ICredentialRepository _credentialRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly PhotoOptions _photoOptions;

        public PhotoController(IWebHostEnvironment host,
            ICourseRepository courseRepository,
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IOptions<PhotoOptions> options,
            IFileService fileService,
            ICredentialRepository credentialRepository,
            ISliderRepository sliderRepository,
            ICheckoutRepository checkoutRepository)
        {
            _host = host;
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
            _config = config;
            _fileService = fileService;
            _credentialRepository = credentialRepository;
            _sliderRepository = sliderRepository;
            _checkoutRepository = checkoutRepository;
            _photoOptions = options.Value;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("courses/{courseId}/photo")]
        public async Task<ActionResult<Photo>> UploadCoursePhoto(int courseId, IFormFile file)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId, true);
            if (course == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");
            if (file.Length > _photoOptions.MaxBytes) return BadRequest("حجم فایل بیش از حد بزرگ است");
            if (!_photoOptions.IsSupported(file.FileName)) return BadRequest("فرمت فایل درست نیست");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Files", course.Id.ToString());
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            try
            {
                await _fileService.UploadAsync(file, fileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var photo = new Photo()
            {
                FileName = fileName
            };

            if (course.Photo != null)
            {
                _fileService.Delete(course.Photo.FileName, uploadFolderPath);
            }

            course.Photo = photo;
            await _unitOfWork.CompleteAsync();
            return Ok(photo);
        }

        [Authorize(Roles = "Salesperson")]
        [HttpPost("salesperson/credential/photo")]
        public async Task<ActionResult<Photo>> UploadCredentialPhoto([FromQuery] int credentialId, IFormFile file, [FromQuery] string usedAs)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var credential = await _credentialRepository.GetSalespersonCredentialAsync(userId, true);
            if (credential == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");
            if (file.Length > _photoOptions.MaxBytes) return BadRequest("حجم فایل بیش از حد بزرگ است");
            if (!_photoOptions.IsSupported(file.FileName)) return BadRequest("فرمت فایل درست نیست");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Credentials", credential.UserName);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            try
            {
                await _fileService.UploadAsync(file, fileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var photo = new Photo()
            {
                FileName = fileName
            };

            if (usedAs.ToUpper() == "IDCARD")
            {
                if (credential.IdCardPhoto != null)
                {
                    _fileService.Delete(credential.IdCardPhoto.FileName, uploadFolderPath);
                }

                credential.IdCardPhoto = photo;
            }
            else if (usedAs.ToUpper() == "BANKCARD")
            {
                if (credential.BankCardPhoto != null)
                {
                    _fileService.Delete(credential.BankCardPhoto.FileName, uploadFolderPath);
                }

                credential.BankCardPhoto = photo;
            }

            await _unitOfWork.CompleteAsync();
            return Ok(photo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("sliders/{sliderId}/photo")]
        public async Task<ActionResult<Photo>> UploadSliderPhoto(int sliderId, IFormFile file)
        {
            var sliderItem = await _sliderRepository.GetSliderItemByIdAsync(sliderId);
            if (sliderItem == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");
            if (file.Length > _photoOptions.MaxBytes) return BadRequest("حجم فایل بیش از حد بزرگ است");
            if (!_photoOptions.IsSupported(file.FileName)) return BadRequest("فرمت فایل درست نیست");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Slider");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            try
            {
                await _fileService.UploadAsync(file, fileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var photo = new Photo()
            {
                FileName = fileName
            };

            if (sliderItem.Photo != null)
            {
                _fileService.Delete(sliderItem.Photo.FileName, uploadFolderPath);
            }

            sliderItem.Photo = photo;
            await _unitOfWork.CompleteAsync();
            return Ok(photo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("checkouts/{checkoutId}/photo")]
        public async Task<ActionResult<Photo>> UploadCheckoutReceiptPhoto(int checkoutId, IFormFile file)
        {
            var checkout = await _checkoutRepository.GetCheckoutWithIdAsync(checkoutId);
            if (checkout == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");
            if (file.Length > _photoOptions.MaxBytes) return BadRequest("حجم فایل بیش از حد بزرگ است");
            if (!_photoOptions.IsSupported(file.FileName)) return BadRequest("فرمت فایل درست نیست");

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Checkouts", checkout.UserName);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            try
            {
                await _fileService.UploadAsync(file, fileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var photo = new Photo()
            {
                FileName = fileName
            };

            if (checkout.ReceiptPhoto != null)
            {
                _fileService.Delete(checkout.ReceiptPhoto.FileName, uploadFolderPath);
            }

            checkout.ReceiptPhoto = photo;
            await _unitOfWork.CompleteAsync();
            return Ok(photo);
        }
    }
}
