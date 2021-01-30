using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.Dtos
{
    public class BasketDto
    {
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceToPay { get; set; }
        public string UserId { get; set; }
        public string SalespersonCouponCode { get; set; }
        public string OtherCouponCode { get; set; }
        public ICollection<EpisodeDto> Episodes { get; set; }

        public BasketDto()
        {
            Episodes = new Collection<EpisodeDto>();
        }
    }
}