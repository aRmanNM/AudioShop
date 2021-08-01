using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using API.Helpers;

namespace API.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }

        public int WaitingTimeBetweenEpisodes { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int Visits { get; set; }

        public DateTime LastEdited { get; set; } = DateTime.Now;

        public Photo Photo { get; set; }
        public ICollection<Episode> Episodes { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<CourseCategory> CourseCategories { get; set; }

        [NotMapped]
        public double? AverageScore { get; set; }
        [NotMapped]
        public ICollection<Category> Categories { get; set;}

        public CourseType CourseType { get; set; }
        public bool IsFeatured { get; set; }

        public Course()
        {
            Episodes = new Collection<Episode>();
            Reviews = new Collection<Review>();
            CourseCategories = new Collection<CourseCategory>();
            Categories = new Collection<Category>();
        }
    }
}