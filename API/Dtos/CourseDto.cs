using System.Collections.Generic;
using API.Helpers;
using API.Models;

namespace API.Dtos
{
    public class CourseDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public string PhotoFileName { get; set; }
        public int WaitingTimeBetweenEpisodes { get; set; } = 0;
        public bool IsActive { get; set; }
        public double? AverageScore { get; set; }
        public ICollection<EpisodeDto> Episodes { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        public CourseType CourseType { get; set; }
        public bool IsFeatured { get; set; }
    }
}