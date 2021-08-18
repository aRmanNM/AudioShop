using System;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IProgressRepository
    {
        Task<Progress> GetCourseProgress(string userId, int courseId);
        Task<Progress> CreateProgress(Progress progress);
        void UpdateProgress(Progress progress);
        Task DeleteProgress(int progressId);
        Task<bool> CheckIfAlreadyListenedAsync(int courseId, string userId);
    }
}