using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Models;

namespace API.Dtos
{
    public class EpisodeDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public decimal Price { get; set; }
        public ICollection<Audio> Audios { get; set; }
        public int CourseId { get; set; }

        public EpisodeDto()
        {
            Audios = new Collection<Audio>();
        }
    }
}