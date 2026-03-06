using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Domain.Entities.Movies;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Repositories.Implementation
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _context.Movies
                .Include(m => m.Ratings)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
        {
            return await _context.Movies
                .Where(m => m.Genres.Contains(genre))
                .Include(m => m.Ratings)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Movie?> GetMovieAsync(Guid id)
        {
            return await _context.Movies
                .Include(m => m.Ratings)
                .ThenInclude(r => r.User)
                .Include(m => m.Screenings)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie> CreateMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            return movie;
        }

        public Task UpdateMovieAsync(Movie movie)
        {
            movie.UpdatedAt = DateTime.UtcNow;
            _context.Movies.Update(movie);
            return Task.CompletedTask;
        }

        public Task DeleteMovieAsync(Movie movie)
        {
            movie.SoftDelete();
            _context.Movies.Update(movie);
            return Task.CompletedTask;
        }

        public async Task<List<string>> GetAllGenresAsync()
        {
            var genres = await _context.Movies
                .Where(m => !string.IsNullOrEmpty(m.Genres))
                .Select(m => m.Genres)
                .Distinct()
                .ToListAsync();

            return genres
                .SelectMany(g => g.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(g => g.Trim())
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }

        public async Task<(decimal WeightedRating, List<MovieRating> Ratings)> GetRatingsForMovieAsync(Guid movieId)
        {
            var ratings = await _context.MovieRatings
                .Where(mr => mr.MovieId == movieId)
                .Include(mr => mr.User)
                .AsNoTracking()
                .ToListAsync();

            if (!ratings.Any())
                return (0, new List<MovieRating>());

            var weightedRating = (decimal)ratings.Average(r => r.Rating);
            return (weightedRating, ratings);
        }

        public async Task<MovieRating?> GetRatingOfUserForMovieAsync(Guid movieId, Guid userId)
        {
            return await _context.MovieRatings
                .AsNoTracking()
                .FirstOrDefaultAsync(mr => mr.MovieId == movieId && mr.UserId == userId);
        }

        public async Task<List<Movie>> GetTopNMoviesAsync(int n)
        {
            var moviesWithRatings = await _context.Movies
                .Include(m => m.Ratings)
                .AsNoTracking()
                .ToListAsync();

            return moviesWithRatings
                .Where(m => m.Ratings != null && m.Ratings.Any())
                .OrderByDescending(m => m.Ratings.Average(r => r.Rating))
                .ThenByDescending(m => m.Ratings.Count)
                .Take(n)
                .ToList();
        }

        public async Task<MovieRating> UpsertRatingAsync(MovieRating rating)
        {
            var existing = await _context.MovieRatings
                .FirstOrDefaultAsync(mr => mr.MovieId == rating.MovieId && mr.UserId == rating.UserId);

            if (existing != null)
            {
                existing.Rating = rating.Rating;
                existing.Comment = rating.Comment;
                existing.UpdatedAt = DateTime.UtcNow;
                _context.MovieRatings.Update(existing);
                return existing;
            }
            else
            {
                rating.CreatedAt = DateTime.UtcNow;
                await _context.MovieRatings.AddAsync(rating);
                return rating;
            }
        }
    }
}
