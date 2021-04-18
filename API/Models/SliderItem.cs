namespace API.Models
{
    public class SliderItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CourseId { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public Photo Photo { get; set; }
    }
}