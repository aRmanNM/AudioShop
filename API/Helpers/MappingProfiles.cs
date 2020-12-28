using Core.Dtos;
using Core.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CourseEpisode, CourseEpisodeDto>()
                .ForMember(x => x.Course, y => y.MapFrom(z => z.Course.Name))
                .ForMember(x => x.FileUrl, y => y.MapFrom<CourseEpisodeUrlResolver>());
            CreateMap<Course, CourseDto>().
                ForMember(x => x.PictureUrl, y => y.MapFrom<CourseUrlResolver>());
            CreateMap<RegisterDto, User>()
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.UserName, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.DisplayName, y => y.MapFrom(z => z.DisplayName));
        }
    }
}