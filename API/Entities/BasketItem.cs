namespace API.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CourseId { get; set; }
        public int Price { get; set; }
    }
}