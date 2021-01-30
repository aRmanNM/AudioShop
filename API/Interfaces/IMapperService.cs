using System.Threading.Tasks;
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IMapperService
    {
        // Model to DTO
        CourseDto MapCourseToCourseDto(Course course);
        OrderForSalespersonDto MapOrderToOrderForSalespersonDto(Order order, int salePercent);
        EpisodeDto MapEpisodeToEpisodeDto(Episode episode);

        // DTO to Model
        User MapRegisterDtoToUser(RegisterDto registerDto);
        Task<UserDto> MapUserToUserDto(User user);
        Course MapCourseDtoToCourse(CourseDto courseDto);
        Episode MapEpisodeDtoToEpisode(EpisodeDto episodeDto);

    }
}
