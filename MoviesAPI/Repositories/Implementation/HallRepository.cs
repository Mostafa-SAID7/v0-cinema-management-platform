using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Domain.Entities.Halls;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Repositories.Implementation
{
    public class HallRepository : IHallRepository
    {
        private readonly ApplicationDbContext _context;

        public HallRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hall>> GetAllHallsAsync()
        {
            return await _context.Halls
                .Include(h => h.HallSeats)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Hall?> GetHallByIdAsync(Guid hallId)
        {
            return await _context.Halls
                .Include(h => h.HallSeats)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == hallId);
        }

        public async Task<List<HallSeat>> GetSeatsByHallIdAsync(Guid hallId)
        {
            return await _context.HallSeats
                .Where(hs => hs.HallId == hallId)
                .AsNoTracking()
                .OrderBy(hs => hs.RowNumber)
                .ThenBy(hs => hs.SeatNumber)
                .ToListAsync();
        }

        public async Task<HallSeat?> GetSeatByIdAsync(Guid seatId)
        {
            return await _context.HallSeats
                .AsNoTracking()
                .FirstOrDefaultAsync(hs => hs.Id == seatId);
        }
    }
}
