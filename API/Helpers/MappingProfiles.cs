using Core.Dtos;
using Core.Entities;
using AutoMapper;
using Core.Interfaces;
using System.Threading.Tasks;

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
            CreateMap<RegisterDto, User>();
        }
    }
}