﻿using System.Threading.Tasks;
using API.Dtos;
using API.Models;
using API.Models.Ads;
using API.Models.Landing;

namespace API.Interfaces
{
    public interface IMapperService
    {
        // Model to DTO
        CourseDto MapCourseToCourseDto(Course course);
        EpisodeDto MapEpisodeToEpisodeDto(Episode episode);
        SliderItemDto MapSliderItemToSliderItemDto(SliderItem sliderItem);
        SalespersonDto MapUserToSalespersonDto(User user);
        ReviewDto MapReviewToReviewDto(Review review);
        CategoryDto MapCategoryToCategoryDto(Category category);
        StatDto MapStatToStatDto(Stat stat);
        OrderWithUserInfo MapOrderToOrderWithUserInfo(Order order);
        AdDto MapAdToAdDto(Ad ad);

        // DTO to Model
        User MapRegisterDtoToUser(RegisterDto registerDto);
        Task<UserDto> MapUserToUserDtoAsync(User user);
        Course MapCourseDtoToCourse(CourseDto courseDto);
        Episode MapEpisodeDtoToEpisode(EpisodeDto episodeDto);
        Task<Coupon> MapCouponDtoToCouponAsync(CouponToCreateDto couponDto);
        SliderItem MapSliderItemDtoToSliderItem(SliderItemDto sliderItemDto);
        Landing MapLandingDtoToLanding(LandingDto landingDto);
        Ad MapAdDtoToAd(AdDto adDto);
    }
}
