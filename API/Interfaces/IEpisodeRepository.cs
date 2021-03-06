﻿using System;
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
        void UpdateEpisodes(Episode[] episodes);
        Task<IEnumerable<Episode>> GetCourseEpisodes(int courseId);
        Task<Episode> GetEpisodeById(int id);
        void DeleteEpisode(Episode episode);
        Task<IEnumerable<Episode>> GetUserEpisodes(string userId);
        Task<IEnumerable<int>>  GetUserEpisodeIds(string userId);
    }
}
