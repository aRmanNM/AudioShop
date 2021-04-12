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

        public async Task<PaginatedResult<ReviewDto>> GetCourseReviewsAsync(int? courseId, bool accepted = true, int pageNumber = 1, int pageSize = 10)
        {

            if (pageSize > 20 || pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var reviews = _context.Reviews.AsQueryable();

            if (courseId == null)
            {
                reviews = reviews.Where(r => r.Accepted == accepted).AsQueryable();
            }
            else
            {
                reviews = reviews.Where(r => r.CourseId == courseId && r.Accepted == accepted);
            }

            var result = new PaginatedResult<ReviewDto>();
            result.TotalItems = reviews.Count();
            result.Items = await reviews
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Text = r.Text,
                    Rating = r.Rating,
                    Date = r.Date,
                    CourseName = r.Course.Name,
                    Accepted = r.Accepted,
                    UserFirstAndLastName = (r.User.FirstName == null && r.User.LastName == null) ? "کاربر ناشناس" : $"{r.User.FirstName} {r.User.LastName}"
                })
                .OrderByDescending(r => r.Date)
                .AsNoTracking()
                .ToArrayAsync();

            return result;
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);                
            return review;
        }

        public async Task UpdateReview(ReviewDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(reviewDto.Id);

            review.Text = reviewDto.Text;
            review.Accepted = reviewDto.Accepted;
            review.Rating = reviewDto.Rating;
            
            _context.Reviews.Update(review);            
        }

        public async Task<PaginatedResult<ReviewDto>> GetAllReviewsAsync(bool accepted, int pageNumber = 1, int pageSize = 10)
        {
            return await GetCourseReviewsAsync(null, accepted, pageNumber, pageSize);
        }

        public async Task AcceptMultipleReviews(int[] reviewIds)
        {
            var reviews = await _context.Reviews.Where(r => reviewIds.Any(p => p == r.Id)).ToArrayAsync();
            foreach (var review in reviews)
            {
                review.Accepted = true;
            }
        }
    }
}
