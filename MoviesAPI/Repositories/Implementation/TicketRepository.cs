using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Domain.Entities.Tickets;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Repositories.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Movie)
                .Include(t => t.User)
                .Include(t => t.HallSeat)
                .ThenInclude(hs => hs.Hall)
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketAsync(Guid id)
        {
            return await _context.Tickets
                .Include(t => t.Movie)
                .Include(t => t.User)
                .Include(t => t.HallSeat)
                .ThenInclude(hs => hs.Hall)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            ticket.CreatedAt = DateTime.UtcNow;
            await _context.Tickets.AddAsync(ticket);
            return ticket;
        }

        public Task UpdateTicketAsync(Ticket ticket)
        {
            ticket.UpdatedAt = DateTime.UtcNow;
            _context.Tickets.Update(ticket);
            return Task.CompletedTask;
        }

        public Task DeleteTicketAsync(Ticket ticket)
        {
            ticket.SoftDelete();
            _context.Tickets.Update(ticket);
            return Task.CompletedTask;
        }

        public async Task<int> GetPurchasedTicketsCountAsync(Guid movieId, DateTime showTime)
        {
            return await _context.Tickets
                .CountAsync(t => t.MovieId == movieId && t.WatchDateTime == showTime);
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(Guid userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .Include(t => t.Movie)
                .Include(t => t.HallSeat)
                .ThenInclude(hs => hs.Hall)
                .AsNoTracking()
                .OrderByDescending(t => t.WatchDateTime)
                .ToListAsync();
        }
    }
}
