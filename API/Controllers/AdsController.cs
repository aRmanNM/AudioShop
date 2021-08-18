using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using API.Models.Ads;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly IAdRepository _adRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RandomService _randomService;
        private readonly IWebHostEnvironment _host;
        private readonly IFileService _fileService;
        private readonly IMapperService _mapper;

        public AdsController(IAdRepository adRepository,
            IUnitOfWork unitOfWork,
            RandomService randomService,
            IWebHostEnvironment host,
            IFileService fileService,
            IMapperService mapper)
        {
            _randomService = randomService;
            _host = host;
            _fileService = fileService;
            _mapper = mapper;
            _adRepository = adRepository;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ad>>> GetAdds()
        {
            var ads = await _adRepository.GetAdsAsync();
            return Ok(ads.Select(a => _mapper.MapAdToAdDto(a)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{adId}")]
        public async Task<ActionResult<Ad>> GetAddById(int adId)
        {
            var ad = await _adRepository.GetAdByIdAsync(adId);
            return Ok(_mapper.MapAdToAdDto(ad));
        }


        [HttpGet("titles/{titleEn}")]
        public async Task<ActionResult<Ad>> GetAddByTitleEn(string titleEn)
        {
            var ads = (await _adRepository.GetAdsByTitleEnAsync(titleEn)).ToList();
            if (ads == null || ads.Count == 0)
            {
                return NoContent();
            }

            var randomAd = ads[_randomService.random.Next(ads.Count())];
            return Ok(_mapper.MapAdToAdDto(randomAd));
        }

        [HttpGet("places")]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            var places = await _adRepository.GetPlacesAsync();
            return Ok(places);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Ad>> CreateAd(AdDto adDto)
        {
            var ad = _mapper.MapAdDtoToAd(adDto);
            // var places = await _adRepository.GetPlacesByIdsAsync(adCreateDto.AdPlaceIds.ToList());
            foreach (var place in adDto.Places)
            {
                ad.AdPlaces.Add(new AdPlace { PlaceId = place.Id, AdId = ad.Id });
            }

            await _adRepository.CreateAdAsync(ad);
            await _unitOfWork.CompleteAsync();
            return Ok(ad);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("places/{placeId}")]
        public async Task<ActionResult<Place>> EditPlace(int placeId, Place place)
        {
            _adRepository.EditPlace(place);
            await _unitOfWork.CompleteAsync();
            return Ok(place);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{adId}")]
        public async Task<ActionResult<AdDto>> EditAd(int adId, AdDto adDto)
        {
            var ad = _mapper.MapAdDtoToAd(adDto);
            _adRepository.EditAd(ad);
            await _adRepository.DeleteAdPlaces(adId);
            await _unitOfWork.CompleteAsync();

            var adPlaces = adDto.Places.Select(p => new AdPlace { PlaceId = p.Id, AdId = ad.Id }).ToList();
            await _adRepository.AdddAdPlaces(adPlaces);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.MapAdToAdDto(ad));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{adId}/file")]
        public async Task<ActionResult<ContentFile>> UploadFile(int adId, IFormFile file)
        {
            var ad = await _adRepository.GetAdByIdAsync(adId);
            if (ad == null) return NotFound();
            if (file == null) return BadRequest("null file");
            if (file.Length == 0) return BadRequest("empty file");
            // TODO: Add Format Validation

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Ads", adId.ToString());
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            try
            {
                await _fileService.UploadAsync(file, fileName, uploadFolderPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var media = new ContentFile
            {
                FileName = fileName
            };

            ad.File = media;
            await _unitOfWork.CompleteAsync();
            return Ok(media);
        }
    }
}