using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly StoreContext _context;

        public ReviewRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Review>> GetCourseReviewsAsync(int courseId, Boolean accepted)
        {
            var reviews = _context.Reviews.AsQueryable();

            reviews = accepted ? reviews.Where(r => r.Accepted == true) : reviews.Where(r => r.Accepted == false);
            return await reviews
                .Where(r => r.CourseId == courseId)
                .OrderByDescending(r => r.Date)
                .ToArrayAsync();
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            return review;
        }

        public Review UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            return review;
        }

        public async Task<ICollection<Review>> GetAllReviewsAsync(bool accepted)
        {
            return await _context.Reviews
                .Where(r => r.Accepted == accepted)
                .OrderByDescending(r => r.Date)
                .ToArrayAsync();
        }
    }
}
