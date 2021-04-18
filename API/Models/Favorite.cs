namespace API.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public int? EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }

    }
}