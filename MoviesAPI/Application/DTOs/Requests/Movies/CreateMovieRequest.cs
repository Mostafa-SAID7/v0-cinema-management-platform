namespace MoviesAPI.Application.DTOs.Requests.Movies
{
    /// <summary>
    /// Request DTO for creating a new movie
    /// </summary>
    public class CreateMovieRequest
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Amount { get; set; }
        public string PosterPath { get; set; }
        public string Plot { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
        public List<string> Genres { get; set; }
    }

    /// <summary>
    /// Request DTO for updating an existing movie
    /// </summary>
    public class UpdateMovieRequest
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Amount { get; set; }
        public string PosterPath { get; set; }
        public string Plot { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
        public List<string> Genres { get; set; }
    }

    /// <summary>
    /// Request DTO for rating a movie
    /// </summary>
    public class RateMovieRequest
    {
        public long MovieId { get; set; }
        public long UserId { get; set; }
        public int Rating { get; set; } // 1-10
    }

    /// <summary>
    /// Request DTO for getting movies with filters
    /// </summary>
    public class GetMoviesRequest
    {
        public string SearchTerm { get; set; }
        public string Genre { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public DateTime? ReleaseDateFrom { get; set; }
        public DateTime? ReleaseDateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Name"; // Name, ReleaseDate, Rating
        public bool SortDescending { get; set; } = false;
    }
}
