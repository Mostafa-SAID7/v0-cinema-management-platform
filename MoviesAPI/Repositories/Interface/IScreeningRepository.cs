using MoviesAPI.Domain.Entities.Screenings;
using MoviesAPI.Domain.Entities.Halls;

namespace MoviesAPI.Repositories.Interface
{
    public interface IScreeningRepository
    {
        Task<IEnumerable<Screening>> GetScreeningsAsync();
        Task<Screening?> GetScreeningAsync(Guid id);
        Task<Screening> CreateScreeningAsync(Screening screening);
        Task UpdateScreeningAsync(Screening screening);
        Task DeleteScreeningAsync(Screening screening);
        Task<List<HallSeat>> GetReservedSeatsAsync(Guid screeningId);
        Task BookSeatsAsync(Guid screeningId, Guid userId, List<Guid> hallSeatIds);
        Task<List<Screening>> GetScreeningsByHallAndDateAsync(Guid hallId, DateOnly date);
        Task<List<Screening>> GetScreeningsByDateAsync(DateOnly date);
    }
}
