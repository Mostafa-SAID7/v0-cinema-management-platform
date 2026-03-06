using MoviesAPI.Domain.Entities.Movies;

namespace MoviesAPI.Repositories.Interface
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre);
        Task<Movie?> GetMovieAsync(Guid id);
        Task<Movie> CreateMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(Movie movie);
        Task<List<string>> GetAllGenresAsync();
        Task<(decimal WeightedRating, List<MovieRating> Ratings)> GetRatingsForMovieAsync(Guid movieId);
        Task<MovieRating?> GetRatingOfUserForMovieAsync(Guid movieId, Guid userId);
        Task<List<Movie>> GetTopNMoviesAsync(int n);
        Task<MovieRating> UpsertRatingAsync(MovieRating rating);
    }
}
