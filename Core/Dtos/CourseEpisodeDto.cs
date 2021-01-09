namespace Core.Dtos
{
    public class CourseEpisodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string FileUrl { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public string Course { get; set; }
    }
}