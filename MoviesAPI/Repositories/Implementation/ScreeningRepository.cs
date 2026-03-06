using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Domain.Entities.Screenings;
using MoviesAPI.Domain.Entities.Halls;
using MoviesAPI.Domain.Entities.Tickets;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Repositories.Implementation
{
    public class ScreeningRepository : IScreeningRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreeningRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Screening>> GetScreeningsAsync()
        {
            return await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .AsNoTracking()
                .OrderBy(s => s.ScreeningDateTime)
                .ToListAsync();
        }

        public async Task<Screening?> GetScreeningAsync(Guid id)
        {
            return await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .ThenInclude(h => h.HallSeats)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Screening> CreateScreeningAsync(Screening screening)
        {
            screening.CreatedAt = DateTime.UtcNow;
            await _context.Screenings.AddAsync(screening);
            return screening;
        }

        public Task UpdateScreeningAsync(Screening screening)
        {
            screening.UpdatedAt = DateTime.UtcNow;
            _context.Screenings.Update(screening);
            return Task.CompletedTask;
        }

        public Task DeleteScreeningAsync(Screening screening)
        {
            screening.SoftDelete();
            _context.Screenings.Update(screening);
            return Task.CompletedTask;
        }

        public async Task<List<HallSeat>> GetReservedSeatsAsync(Guid screeningId)
        {
            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == screeningId);

            if (screening == null)
                return new List<HallSeat>();

            var reservedSeatIds = await _context.Tickets
                .Where(t => t.MovieId == screening.MovieId && 
                           t.WatchDateTime == screening.ScreeningDateTime)
                .Select(t => t.HallSeatId)
                .ToListAsync();

            return await _context.HallSeats
                .Where(hs => reservedSeatIds.Contains(hs.Id))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task BookSeatsAsync(Guid screeningId, Guid userId, List<Guid> hallSeatIds)
        {
            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.Id == screeningId);

            if (screening == null)
                throw new InvalidOperationException("Screening not found");

            if (screening.AvailableTickets < hallSeatIds.Count)
                throw new InvalidOperationException("Not enough available tickets");

            var tickets = hallSeatIds.Select(seatId => new Ticket
            {
                Id = Guid.NewGuid(),
                MovieId = screening.MovieId,
                UserId = userId,
                WatchDateTime = screening.ScreeningDateTime,
                Price = screening.Movie?.Amount ?? 0,
                HallSeatId = seatId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _context.Tickets.AddRangeAsync(tickets);

            screening.AvailableTickets -= hallSeatIds.Count;
            screening.UpdatedAt = DateTime.UtcNow;
            _context.Screenings.Update(screening);
        }

        public async Task<List<Screening>> GetScreeningsByHallAndDateAsync(Guid hallId, DateOnly date)
        {
            var startDate = date.ToDateTime(TimeOnly.MinValue);
            var endDate = date.ToDateTime(TimeOnly.MaxValue);

            return await _context.Screenings
                .Where(s => s.HallId == hallId && 
                           s.ScreeningDateTime >= startDate && 
                           s.ScreeningDateTime <= endDate)
                .Include(s => s.Movie)
                .AsNoTracking()
                .OrderBy(s => s.ScreeningDateTime)
                .ToListAsync();
        }

        public async Task<List<Screening>> GetScreeningsByDateAsync(DateOnly date)
        {
            var startDate = date.ToDateTime(TimeOnly.MinValue);
            var endDate = date.ToDateTime(TimeOnly.MaxValue);

            return await _context.Screenings
                .Where(s => s.ScreeningDateTime >= startDate && 
                           s.ScreeningDateTime <= endDate)
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .AsNoTracking()
                .OrderBy(s => s.ScreeningDateTime)
                .ToListAsync();
        }
    }
}
