using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(18)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18)")]
        public decimal PriceToPay { get; set; }

        public bool Status { get; set; }
        public string PaymentReceipt { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18)")]
        public decimal SalespersonShare { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string SalespersonCouponCode { get; set; }
        public string OtherCouponCode { get; set; }
        public ICollection<OrderEpisode> OrderEpisodes { get; set; }

        public Order()
        {
            OrderEpisodes = new Collection<OrderEpisode>();
        }
    }
}