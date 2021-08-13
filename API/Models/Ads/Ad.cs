using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Helpers;

namespace API.Models.Ads
{
    public class Ad
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public ContentFile File { get; set; }
        public AdType AdType { get; set; }
        public ICollection<AdPlace> AdPlaces { get; set; }
        public bool IsEnabled { get; set; }

        public Ad()
        {
            AdPlaces = new Collection<AdPlace>();
        }
    }
}