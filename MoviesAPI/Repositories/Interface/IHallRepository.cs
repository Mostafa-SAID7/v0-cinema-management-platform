using MoviesAPI.Domain.Entities.Halls;

namespace MoviesAPI.Repositories.Interface
{
    public interface IHallRepository
    {
        Task<List<Hall>> GetAllHallsAsync();
        Task<Hall?> GetHallByIdAsync(Guid hallId);
        Task<List<HallSeat>> GetSeatsByHallIdAsync(Guid hallId);
        Task<HallSeat?> GetSeatByIdAsync(Guid seatId);
    }
}
