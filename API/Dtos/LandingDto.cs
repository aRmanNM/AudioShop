using API.Models;

namespace API.Dtos
{
    public class LandingDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string UrlName { get; set; }
        public Photo Logo { get; set; }
        public bool LogoEnabled { get; set; }

        public string Title { get; set; }
        public bool TitleEnabled { get; set; }

        public ContentFile Media { get; set; }
        public bool MediaEnabled { get; set; }

        public string Text1 { get; set; }
        public bool Text1Enabled { get; set; }

        public string Button { get; set; }
        public string ButtonLink { get; set; }
        public bool ButtonEnabled { get; set; }
        public int ButtonClickCount { get; set; }

        public string Text2 { get; set; }
        public bool Text2Enabled { get; set; }

        public string Gift { get; set; }
        public bool GiftEnabled { get; set; }

        public bool PhoneBoxEnabled { get; set; }

        public int PhoneNumberCounts { get; set; }

        public string ButtonsColor { get; set; }
        public Photo Background { get; set; }
    }
}