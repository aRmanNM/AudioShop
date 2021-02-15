using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetCourseReviews(int courseId, Boolean accepted = true);
        Task<Review> AddReview(Review review);
        Review UpdateReview(Review review);
    }
}
