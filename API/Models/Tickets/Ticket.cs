using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Helpers;

namespace API.Models.Tickets
{
    public class Ticket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public TicketStatus TicketStatus { get; set; }
        public ICollection<TicketResponse> TicketResponses { get; set; }

        public Ticket()
        {
            TicketResponses = new Collection<TicketResponse>();
        }
    }
}