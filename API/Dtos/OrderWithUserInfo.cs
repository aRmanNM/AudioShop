using System;
using API.Models;

namespace API.Dtos
{
    public class OrderWithUserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceToPay { get; set; }
        public bool Status { get; set; }
        public string PaymentReceipt { get; set; }
        public DateTime Date { get; set; }
        public string SalespersonCouponCode { get; set; }
        public string OtherCouponCode { get; set; }
        public string MemberName { get; set; }
    }
}