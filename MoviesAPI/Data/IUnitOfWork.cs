using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        ITicketRepository Tickets { get; }
        IScreeningRepository Screenings { get; }
        IHallRepository Halls { get; }
        IChatBotRepository ChatBot { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
