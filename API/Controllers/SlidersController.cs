using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
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
        public SlidersController(ISliderRepository sliderRepository, IUnitOfWork unitOfWork, IMapperService mapper)
        {
            _mapper = mapper;
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
        public async Task<ActionResult<SliderItem>> CreateSliderItem(SliderItem sliderItem)
        {
            var sliderItems = await _sliderRepository.CreateSliderItem(sliderItem);
            await _unitOfWork.CompleteAsync();
            return Ok(sliderItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<SliderItem>> UpdateSliderItem(SliderItem sliderItem)
        {
            var sliderItems = _sliderRepository.UpdateSliderItem(sliderItem);
            await _unitOfWork.CompleteAsync();
            return Ok(sliderItem);
        }
    }
}