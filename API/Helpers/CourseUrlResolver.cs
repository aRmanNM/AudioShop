using API.Dtos;
using API.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class CourseUrlResolver : IValueResolver<Course, CourseDto, string>
    {
        private readonly IConfiguration _config;
        public CourseUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Course source, CourseDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}