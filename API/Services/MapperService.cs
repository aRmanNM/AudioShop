using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class MapperService : IMapperService
    {
        private readonly ITokenService _tokenService;
        private readonly IConfigRepository _configRepository;
        private readonly ICouponRepository _couponRepository;

        public MapperService(ITokenService tokenService,
            IConfigRepository configRepository,
            ICouponRepository couponRepository)
        {
            _tokenService = tokenService;
            _configRepository = configRepository;
            _couponRepository = couponRepository;
        }

        public User MapRegisterDtoToUser(RegisterDto registerDto)
        {
            return new User()
            {
                UserName = registerDto.UserName.ToLower(),
                PhoneNumber = registerDto.PhoneNumber,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                City = registerDto.City,
                Country = registerDto.Country,
                Age = registerDto.Age,
                Gender = registerDto.Gender
            };
        }

        public async Task<UserDto> MapUserToUserDtoAsync(User user)
        {
            return new UserDto
            {
                Token = await _tokenService.CreateTokenAsync(user),
                HasPhoneNumber = user.PhoneNumber != null,
                SalespersonCouponCode = user.CouponCode
            };
        }

        public Course MapCourseDtoToCourse(CourseDto courseDto)
        {
            return new Course
            {
                Name = courseDto.Name,
                Price = courseDto.Price,
                Description = courseDto.Description,
                WaitingTimeBetweenEpisodes = courseDto.WaitingTimeBetweenEpisodes,
                IsActive = courseDto.IsActive
            };
        }

        public CourseDto MapCourseToCourseDto(Course course)
        {
            return new CourseDto()
            {
                Description = course.Description,
                Id = course.Id,
                Name = course.Name,
                Price = course.Price,
                PhotoFileName = course.Photo?.FileName,
                WaitingTimeBetweenEpisodes = course.WaitingTimeBetweenEpisodes,
                IsActive = course.IsActive,
                Episodes = course.Episodes.Select(MapEpisodeToEpisodeDto).ToList()
            };
        }

        public EpisodeDto MapEpisodeToEpisodeDto(Episode episode)
        {
            return new EpisodeDto
            {
                Id = episode.Id,
                Name = episode.Name,
                Description = episode.Description,
                Price = episode.Price,
                Sort = episode.Sort,
                Audios = episode.Audios,
                CourseId = episode.CourseId,
                TotalAudiosDuration = episode.TotalAudiosDuration
            };
        }

        public Episode MapEpisodeDtoToEpisode(EpisodeDto episodeDto)
        {
            return new Episode
            {
                Name = episodeDto.Name,
                Description = episodeDto.Description,
                Sort = episodeDto.Sort,
                Price = episodeDto.Price,
                CourseId = episodeDto.CourseId,
                Audios = episodeDto.Audios,
                TotalAudiosDuration = episodeDto.TotalAudiosDuration
            };
        }

        public async Task<Coupon> MapCouponDtoToCouponAsync(CouponToCreateDto couponDto)
        {
            var config = await _configRepository.GetConfigAsync("DefaultDiscountPercentage");
            return new Coupon
            {
                Description = couponDto.Description,
                DiscountPercentage = couponDto.DiscountPercentage ?? int.Parse(config.Value),
                Code = await _couponRepository.GenerateCouponCodeAsync(),
                IsActive = couponDto.IsActive
            };
        }

        public SliderItemDto MapSliderItemToSliderItemDto(SliderItem slideritem)
        {
            return new SliderItemDto
            {
                Id = slideritem.Id,
                Title = slideritem.Title,
                Description = slideritem.Description,
                CourseId = slideritem.CourseId,
                IsActive = slideritem.IsActive,
                PhotoFileName = slideritem.Photo?.FileName ?? null
            };
        }

        public SalespersonDto MapUserToSalespersonDto(User user)
        {
            return new SalespersonDto {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Country = user.Country,
                Age = user.Age,
                Gender = user.Gender,
                SalePercentageOfSalesperson = user.SalePercentageOfSalesperson,
                CurrentSalesOfSalesperson = user.CurrentSalesOfSalesperson,
                TotalSalesOfSalesperson = user.TotalSalesOfSalesperson,
                CouponCode = user.CouponCode,
                SalespersonCredential = user.SalespersonCredential,
                CredentialAccepted = user.CredentialAccepted
            };
        }

        public SliderItem MapSliderItemDtoToSliderItem(SliderItemDto sliderItemDto)
        {
            return new SliderItem {
                Title = sliderItemDto.Title,
                Description = sliderItemDto.Description,
                IsActive = sliderItemDto.IsActive,
                CourseId = sliderItemDto.CourseId
            };
        }
    }
}