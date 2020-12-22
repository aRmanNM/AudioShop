namespace API.Entities
{
    public class CourseEpisode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string FileUrl { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}