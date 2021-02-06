namespace API.Dtos
{
    public class CouponToCreateDto
    {
        public int? DiscountPercentage { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
