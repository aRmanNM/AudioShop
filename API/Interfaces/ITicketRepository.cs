using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;
using API.Models.Tickets;

namespace API.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTickets(bool finished = false);
        Task<Ticket> GetTicketWithResponsesById(int ticketId);
        Task<Ticket> GetTicketById(int ticketId);
        Task<Ticket> CreateTicket(Ticket ticket);
        Task<TicketResponse> CreateResponse(TicketResponse ticketResponse);
        Ticket EditTicket(Ticket ticket);
        Task<Ticket> DeleteTicket(int ticketId);
        Task<IEnumerable<Ticket>> GetUserTickets(string userId);
    }
}