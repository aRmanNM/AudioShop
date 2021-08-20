using System;

namespace API.Models.Tickets
{
    public class TicketResponse
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IssuedByAdmin { get; set; }
    }
}