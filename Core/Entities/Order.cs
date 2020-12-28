using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TotalPrice { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}