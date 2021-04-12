using System;

namespace API.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public short Rating { get; set; }
        public bool Accepted { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;       
        public string CourseName { get; set; }
        public string UserFirstAndLastName { get; set; }
    }
}
