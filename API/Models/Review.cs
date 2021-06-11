using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public short Rating { get; set; }
        public bool Accepted { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string AdminMessage { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
