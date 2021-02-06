namespace API.Models
{
    public class OrderEpisode
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
    }
}
