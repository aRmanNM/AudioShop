using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Models.Tickets;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly StoreContext _context;

        public TicketRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<TicketResponse> CreateResponse(TicketResponse ticketResponse)
        {
            await _context.TicketResponses.AddAsync(ticketResponse);
            return ticketResponse;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            return ticket;
        }

        public async Task<Ticket> DeleteTicket(int ticketId)
        {
            var ticket = await GetTicketById(ticketId);
            _context.Tickets.Remove(ticket);
            return ticket;
        }

        public Ticket EditTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            return ticket;
        }

        public async Task<Ticket> GetTicketById(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task<IEnumerable<Ticket>> GetTickets(bool finished = false)
        {
            if (finished)
            {
                return await _context.Tickets.Where(t => t.TicketStatus == TicketStatus.Finished).ToListAsync();
            }

            return await _context.Tickets.Where(t => t.TicketStatus == TicketStatus.Pending ||
                t.TicketStatus == TicketStatus.AdminAnswered || t.TicketStatus == TicketStatus.MemberAnswered).ToListAsync();
        }

        public async Task<Ticket> GetTicketWithResponsesById(int ticketId)
        {
            return await _context.Tickets.Include(t => t.TicketResponses).FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task<IEnumerable<Ticket>> GetUserTickets(string userId)
        {
            return await _context.Tickets.Where(t => t.UserId == userId).ToListAsync();
        }
    }
}