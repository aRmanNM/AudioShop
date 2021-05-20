using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<CourseCategory> CourseCategories { get; set; }

        public Category()
        {
            CourseCategories = new Collection<CourseCategory>();
        }
    }
}