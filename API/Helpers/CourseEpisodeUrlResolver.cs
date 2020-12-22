using API.Dtos;
using API.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class CourseEpisodeUrlResolver : IValueResolver<CourseEpisode, CourseEpisodeDto, string>
    {
        private readonly IConfiguration _config;
        public CourseEpisodeUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(CourseEpisode source, CourseEpisodeDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.FileUrl))
            {
                return _config["ApiUrl"] + source.FileUrl;
            }

            return null;
        }
    }
}