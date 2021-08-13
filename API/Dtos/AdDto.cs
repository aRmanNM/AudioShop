using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Helpers;
using API.Models;
using API.Models.Ads;

namespace API.Dtos
{
    public class AdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public AdType AdType { get; set; }
        public ContentFile File { get; set; }
        public ICollection<Place> Places { get; set; }
        public bool IsEnabled { get; set; }

        public AdDto()
        {
            Places = new Collection<Place>();
        }
    }
}