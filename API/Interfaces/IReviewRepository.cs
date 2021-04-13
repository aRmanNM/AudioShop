using System.Threading.Tasks;
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IReviewRepository
    {
        Task<PaginatedResult<ReviewDto>> GetCourseReviewsAsync(int? courseId, bool accepted = true, int pageNumber = 1, int pageSize = 10);
        Task<Review> AddReviewAsync(Review review);
        Task UpdateReview(ReviewDto reviewDto);
        Task<PaginatedResult<ReviewDto>> GetAllReviewsAsync(bool accepted, int pageNumber = 1, int pageSize = 10);
        Task ToggleMultipleReviews(int[] reviewIds);
        void DeleteMultipleReviews(int[] reviewIds);
    }
}
