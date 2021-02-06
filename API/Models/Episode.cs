using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        [Column(TypeName = "decimal(18)")]
        public decimal Price { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Audio> Audios { get; set; }
        public ICollection<OrderEpisode> OrderEpisodes { get; set; }

        public int TotalAudiosDuration { get; set; }

        public Episode()
        {
            Audios = new Collection<Audio>();
            OrderEpisodes = new Collection<OrderEpisode>();
        }
    }
}