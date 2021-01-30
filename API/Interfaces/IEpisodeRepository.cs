using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IEpisodeRepository
    {
        Task<Episode> CreateEpisode(Episode courseEpisode);
        Episode UpdateEpisode(Episode courseEpisode);
        Task<IEnumerable<Episode>> GetCourseEpisodes(int courseId);
        Task<Episode> GetEpisodeById(int id);
        void DeleteEpisodes(IEnumerable<Episode> IDs); // TODO: WHAT IS THIS!?
        Task<IEnumerable<Episode>> GetUserEpisodes(string userId);
        Task<IEnumerable<int>>  GetUserEpisodeIds(string userId);
    }
}
