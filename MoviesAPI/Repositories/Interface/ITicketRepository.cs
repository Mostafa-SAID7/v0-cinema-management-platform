using MoviesAPI.Domain.Entities.Tickets;

namespace MoviesAPI.Repositories.Interface
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<Ticket?> GetTicketAsync(Guid id);
        Task<Ticket> CreateTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(Ticket ticket);
        Task<int> GetPurchasedTicketsCountAsync(Guid movieId, DateTime showTime);
        Task<List<Ticket>> GetTicketsByUserIdAsync(Guid userId);
    }
}
