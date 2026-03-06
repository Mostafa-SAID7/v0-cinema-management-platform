using Microsoft.EntityFrameworkCore.Storage;
using MoviesAPI.Repositories.Implementation;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            // Initialize repositories (UserRepository is excluded as it's managed by Identity)
            Movies = new MovieRepository(_context);
            Tickets = new TicketRepository(_context);
            Screenings = new ScreeningRepository(_context);
            Halls = new HallRepository(_context);
            ChatBot = new ChatBotRepository(_context);
        }

        public IMovieRepository Movies { get; private set; }
        public ITicketRepository Tickets { get; private set; }
        public IScreeningRepository Screenings { get; private set; }
        public IHallRepository Halls { get; private set; }
        public IChatBotRepository ChatBot { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                    await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
