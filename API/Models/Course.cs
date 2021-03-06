using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18)")]
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int WaitingTimeBetweenEpisodes { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public Photo Photo { get; set; }
        public ICollection<Episode> Episodes { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public Course()
        {
            Episodes = new Collection<Episode>();
            Reviews = new Collection<Review>();
        }
    }
}