using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IEpisodeRepository
    {
        Task<Episode> CreateEpisodeAsync(Episode courseEpisode);
        Episode UpdateEpisode(Episode courseEpisode);
        void UpdateEpisodes(Episode[] episodes);
        Task<IEnumerable<Episode>> GetCourseEpisodesAsync(int courseId);
        Task<Episode> GetEpisodeByIdAsync(int id);
        void DeleteEpisode(Episode episode);
        Task<IEnumerable<Episode>> GetUserEpisodesAsync(string userId);
        Task<IEnumerable<int>>  GetUserEpisodeIdsAsync(string userId);
        Task<bool> CheckIfAlreadyBoughtAsync(Episode episode);
    }
}
