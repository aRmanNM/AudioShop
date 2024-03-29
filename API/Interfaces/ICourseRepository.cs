﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> CreateCourseAsync(Course course);
        Course UpdateCourse(Course course);
        Task<PaginatedResult<Course>> GetCoursesAsync(bool includeEpisodes,
            string search, bool includeInactive = false, int pageNumber = 1, int pageSize = 10, string category = null, CourseType courseType = CourseType.None, bool onlyFeatured = false);
        Task<Course> GetCourseByIdAsync(int id, bool withTracking = false);
        Task DeleteCourseCategories(int courseId);
        Task<IEnumerable<CourseCategory>> AdddCourseCategories(ICollection<CourseCategory> courseCategories);
        Task<IEnumerable<Course>> GetFeaturedCoursesAsync(CourseType courseType = CourseType.Course, int count = 10);
        Task<IEnumerable<Course>> GetTopSellersCoursesAsync(CourseType courseType = CourseType.Course, int count = 10);
        Task<IEnumerable<Course>> GetTopٰClickedCoursesAsync(CourseType courseType = CourseType.Course, int count = 10);
        Task<bool> CheckIfAlreadyBoughtAsync(int courseId, string userId);
    }
}
