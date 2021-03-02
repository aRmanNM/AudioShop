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
        public Boolean Accepted { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}
