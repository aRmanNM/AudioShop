using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlidersController : ControllerBase
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperService _mapper;
        private readonly IWebHostEnvironment _host;
        private readonly IFileService _fileService;

        public SlidersController(ISliderRepository sliderRepository,
            IUnitOfWork unitOfWork,
            IMapperService mapper,
            IWebHostEnvironment host,
            IFileService fileService)
        {
            _mapper = mapper;
            _host = host;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _sliderRepository = sliderRepository;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SliderItemDto>>> GetSliderItems()
        {
            var sliderItems = await _sliderRepository.GetSliderItems();
            var sliderItemDtos = sliderItems.Select(s => _mapper.MapSliderItemToSliderItemDto(s));
            return Ok(sliderItemDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<SliderItemDto>> CreateSliderItem(SliderItemDto sliderItemDto)
        {
            var sliderItem = _mapper.MapSliderItemDtoToSliderItem(sliderItemDto);
            await _sliderRepository.CreateSliderItem(sliderItem);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.MapSliderItemToSliderItemDto(sliderItem));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<SliderItem>> UpdateSliderItem(SliderItem sliderItem)
        {
            var sliderItems = _sliderRepository.UpdateSliderItem(sliderItem);
            await _unitOfWork.CompleteAsync();
            return Ok(sliderItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{sliderItemId}")]
        public async Task<ActionResult> DeleteSliderItem(int sliderItemId)
        {
            var sliderItem = await _sliderRepository.GetSliderItemById(sliderItemId);
            if (sliderItem == null)
            {
                return NotFound();
            }

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Slider");
            _fileService.Delete(sliderItem.Photo.FileName, uploadFolderPath);
            _sliderRepository.DeleteSliderItem(sliderItem);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}