﻿using System;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using API.Models.Ads;
using API.Models.Landing;
using API.Models.Messages;

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
                Gender = registerDto.Gender,
                Employed = registerDto.Employed
            };
        }

        public User MapUserUpdateDtoToUser(UserUpdateDto userUpdateDto)
        {
            return new User()
            {
                Id = userUpdateDto.UserId,
                FirstName = userUpdateDto.FirstName,
                LastName = userUpdateDto.LastName,
                City = userUpdateDto.City,
                Age = userUpdateDto.Age,
                Gender = userUpdateDto.Gender,
                Employed = userUpdateDto.Employed
            };
        }

        public async Task<UserDto> MapUserToUserDtoAsync(User user)
        {
            return new UserDto
            {
                Token = await _tokenService.CreateTokenAsync(user),
                HasPhoneNumber = user.PhoneNumber != null,
                SalespersonCouponCode = user.CouponCode,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Age = user.Age,
                Gender = user.Gender,
                Employed = user.Employed,
                SubscriptionExpirationDate = user.SubscriptionExpirationDate,
                SubscriptionType = user.SubscriptionType
            };
        }

        public Course MapCourseDtoToCourse(CourseDto courseDto)
        {
            return new Course
            {
                Name = courseDto.Name,
                Price = courseDto.Price,
                Description = courseDto.Description,
                Instructor = courseDto.Instructor,
                WaitingTimeBetweenEpisodes = courseDto.WaitingTimeBetweenEpisodes,
                IsActive = courseDto.IsActive,
                CourseType = courseDto.CourseType,
                IsFeatured = courseDto.IsFeatured
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
                Instructor = course.Instructor,
                PhotoFileName = course.Photo?.FileName,
                WaitingTimeBetweenEpisodes = course.WaitingTimeBetweenEpisodes,
                IsActive = course.IsActive,
                AverageScore = course.AverageScore,
                Episodes = course.Episodes.Select(MapEpisodeToEpisodeDto).ToList(),
                Categories = course.CourseCategories.Select(c => MapCategoryToCategoryDto(c.Category)).ToList(),
                CourseType = course.CourseType,
                IsFeatured = course.IsFeatured,
                Visits = course.Visits
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
                Link = slideritem.Link,
                PhotoFileName = slideritem.Photo?.FileName ?? null
            };
        }

        public SalespersonDto MapUserToSalespersonDto(User user)
        {
            return new SalespersonDto
            {
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
                DiscountPercentageOfSalesperson = user.Coupon.DiscountPercentage,
                CouponCode = user.CouponCode,
                SalespersonCredential = user.SalespersonCredential,
                CredentialAccepted = user.CredentialAccepted
            };
        }

        public SliderItem MapSliderItemDtoToSliderItem(SliderItemDto sliderItemDto)
        {
            return new SliderItem
            {
                Title = sliderItemDto.Title,
                Description = sliderItemDto.Description,
                IsActive = sliderItemDto.IsActive,
                CourseId = sliderItemDto.CourseId,
                Link = sliderItemDto.Link
            };
        }

        public ReviewDto MapReviewToReviewDto(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                Text = review.Text,
                Rating = review.Rating,
                Accepted = review.Accepted,
                Date = review.Date,
                AdminMessage = review.AdminMessage,
                CourseName = review.Course.Name,
                UserFirstAndLastName = (review.User.FirstName == null && review.User.LastName == null) ? "کاربر ناشناس" : $"{review.User.FirstName} {review.User.LastName}"
            };
        }

        public CategoryDto MapCategoryToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Title = category.Title
            };
        }

        public StatDto MapStatToStatDto(Stat stat)
        {
            return new StatDto
            {
                TitleEn = stat.TitleEn,
                TitleFa = stat.TitleFa,
                Counter = stat.Counter,
                DateOfStat = stat.DateOfStat
            };
        }

        public OrderWithUserInfo MapOrderToOrderWithUserInfo(Order order)
        {
            return new OrderWithUserInfo
            {
                Id = order.Id,
                UserName = order.User.UserName,
                Status = order.Status,
                Date = order.Date,
                Discount = order.Discount,
                MemberName = order.User.FirstName + ' ' + order.User.LastName,
                OtherCouponCode = order.OtherCouponCode,
                PaymentReceipt = order.PaymentReceipt,
                PriceToPay = order.PriceToPay,
                SalespersonCouponCode = order.SalespersonCouponCode,
                TotalPrice = order.TotalPrice,
                OrderType = order.OrderType
            };
        }

        public Landing MapLandingDtoToLanding(LandingDto landingDto)
        {
            return new Landing
            {
                Id = landingDto.Id,
                Description = landingDto.Description,
                UrlName = landingDto.UrlName,
                Button = landingDto.Button,
                ButtonClickCount = landingDto.ButtonClickCount,
                ButtonEnabled = landingDto.ButtonEnabled,
                ButtonLink = landingDto.ButtonLink,
                GiftEnabled = landingDto.GiftEnabled,
                Gift = landingDto.Gift,
                Logo = landingDto.Logo,
                LogoEnabled = landingDto.LogoEnabled,
                Media = landingDto.Media,
                MediaEnabled = landingDto.MediaEnabled,
                PhoneBoxEnabled = landingDto.PhoneBoxEnabled,
                Text1 = landingDto.Text1,
                Text1Enabled = landingDto.Text1Enabled,
                Text2 = landingDto.Text2,
                Text2Enabled = landingDto.Text2Enabled,
                Title = landingDto.Title,
                TitleEnabled = landingDto.TitleEnabled,
                ButtonsColor = landingDto.ButtonsColor
            };
        }

        public Ad MapAdDtoToAd(AdDto adDto)
        {
            return new Ad
            {
                Id = adDto.Id,
                AdType = adDto.AdType,
                Description = adDto.Description,
                IsEnabled = adDto.IsEnabled,
                Link = adDto.Link,
                Title = adDto.Title
            };
        }

        public AdDto MapAdToAdDto(Ad ad)
        {
            return new AdDto
            {
                Id = ad.Id,
                AdType = ad.AdType,
                Description = ad.Description,
                IsEnabled = ad.IsEnabled,
                Link = ad.Link,
                Places = ad.AdPlaces.Select(ap => ap.Place).ToList(),
                Title = ad.Title,
                File = ad.File
            };
        }

        public MessageDto MapMessageToMessageDto(Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                Body = message.Body,
                Link = message.Link,
                CourseId = message.CourseId,
                Title = message.Title,
                RepeatAfterHour = message.RepeatAfterHour,
                CreatedAt = message.CreatedAt,
                IsRepeatable = message.IsRepeatable,
                MessageType = message.MessageType,
                SendPush = message.SendPush,
                SendSMS = message.SendSMS,
                SendInApp = message.SendInApp,
                UserId = message.UserId
            };
        }

        public Message MapMessageDtoToMessage(MessageDto messageDto)
        {
            return new Message
            {
                Id = messageDto.Id,
                Body = messageDto.Body,
                Link = messageDto.Link,
                CourseId = messageDto.CourseId,
                Title = messageDto.Title,
                RepeatAfterHour = messageDto.RepeatAfterHour,
                IsRepeatable = messageDto.IsRepeatable,
                MessageType = messageDto.MessageType,
                SendPush = messageDto.SendPush,
                SendSMS = messageDto.SendSMS,
                SendInApp = messageDto.SendInApp,
                UserId = messageDto.UserId
            };
        }
    }
}