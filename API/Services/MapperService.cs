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
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                City = registerDto.City,
                Country = registerDto.Country,
                Age = registerDto.Age,
                Gender = registerDto.Gender
            };
        }

        public async Task<UserDto> MapUserToUserDto(User user)
        {
            return new UserDto
            {
                Token = await _tokenService.CreateToken(user),
                HasPhoneNumber = user.PhoneNumber == null
            };
        }

        public Course MapCourseDtoToCourse(CourseDto courseDto)
        {
            return new Course
            {
                Name = courseDto.Name,
                Price = courseDto.Price,
                Description = courseDto.Description
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
                Photo = course.Photo,
                Episodes = course.Episodes.Select(MapEpisodeToEpisodeDto).ToList()
            };
        }

        public OrderForSalespersonDto MapOrderToOrderForSalespersonDto(Order order, int salePercent)
        {
            return new OrderForSalespersonDto()
            {
                // Courses = order.OrderCourses.Select(oc => oc.Course.Name).ToArray(),
                Date = order.Date,
                Price = order.PriceToPay,
                SalespersonShareAmount = order.PriceToPay - ((order.PriceToPay * salePercent) / 100)
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
                CourseId = episode.CourseId
            };
        }

        public Episode MapEpisodeDtoToEpisode(EpisodeDto episodeDto)
        {
            return new Episode {
                Name = episodeDto.Name,
                Description = episodeDto.Description,
                Sort = episodeDto.Sort,
                Price = episodeDto.Price,
                CourseId = episodeDto.CourseId,
                Audios = episodeDto.Audios
            };
        }

        public async Task<Coupon> MapCouponDtoToCoupon(CouponToCreateDto couponDto)
        {
            var config = await _configRepository.GetConfigAsync("DefaultDiscountPercentage");
            return new Coupon
            {
                Description = couponDto.Description,
                DiscountPercentage = couponDto.DiscountPercentage ?? int.Parse(config.Value),
                Code = await _couponRepository.GenerateCouponCode(),
                IsActive = couponDto.IsActive
            };
        }
    }
}