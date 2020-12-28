using System.Collections.Generic;

namespace Core.Dtos
{
    public class BasketDto
    {
        public ICollection<CourseDto> CourseDtos { get; set; }
        public int TotalPrice { get; set; }
        public string UserId { get; set; }
    }
}