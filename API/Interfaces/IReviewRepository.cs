using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetCourseReviewsAsync(int courseId, bool accepted = true);
        Task<Review> AddReviewAsync(Review review);
        Review UpdateReview(Review review);
        Task<ICollection<Review>> GetAllReviewsAsync(bool accepted);
    }
}
