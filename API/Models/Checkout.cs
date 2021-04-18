using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public bool Status { get; set; }

        [Column(TypeName = "decimal(18)")]
        public decimal AmountToCheckout { get; set; }

        public string PaymentReceipt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? PaidAt { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }
        public Photo ReceiptPhoto { get; set; }
    }
}