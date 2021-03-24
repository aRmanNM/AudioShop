using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
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

        public async Task<PaginatedResult<Review>> GetAllReviewsAsync(bool accepted, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > 20 || pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var reviews = _context.Reviews.Where(r => r.Accepted == accepted);
            var result = new PaginatedResult<Review>();
            result.TotalItems = await reviews.CountAsync();
            result.Items = await reviews.OrderByDescending(r => r.Date)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToArrayAsync();

            return result;
        }
    }
}
