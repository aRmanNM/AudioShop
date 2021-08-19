using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Helpers;

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
        public OrderType OrderType { get; set; } = OrderType.Episode;
        public ICollection<int> EpisodeIds { get; set; }

        public BasketDto()
        {
            EpisodeIds = new Collection<int>();
        }
    }
}