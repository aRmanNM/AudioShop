using System.Collections.Generic;

namespace Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}