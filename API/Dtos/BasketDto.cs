using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.Dtos
{
    public class BasketDto
    {
        public ICollection<EpisodeDto> Episodes { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceToPay { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }

        public BasketDto()
        {
            Episodes = new Collection<EpisodeDto>();
        }
    }
}