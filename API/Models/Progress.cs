using System;

namespace API.Models
{
    public class Progress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime LastListened { get; set; }
        public int LastIndex { get; set; }
        public int LastEpisodeId { get; set; }
    }
}