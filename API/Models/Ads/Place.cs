using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Helpers;

namespace API.Models.Ads
{
    public class Place
    {
        public int Id { get; set; }
        public string TitleFa { get; set; }
        public string TitleEn { get; set; }
        public bool IsEnabled { get; set; }
        public AdType AdType { get; set; }
    }
}