namespace API.Models.Ads
{
    public class AdPlace
    {
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public int AdId { get; set; }
        public Ad Ad { get; set; }
    }
}