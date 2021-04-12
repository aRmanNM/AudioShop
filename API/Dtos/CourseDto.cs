using System.Collections.Generic;
using API.Models;

namespace API.Dtos
{
    public class CourseDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string PhotoFileName { get; set; }
        public int WaitingTimeBetweenEpisodes { get; set; } = 0;
        public bool IsActive { get; set; }
        public short? AverageScore { get; set; }
        public ICollection<EpisodeDto> Episodes { get; set; }        
    }
}