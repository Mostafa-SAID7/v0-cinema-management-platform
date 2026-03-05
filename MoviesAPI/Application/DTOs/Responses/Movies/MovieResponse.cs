namespace MoviesAPI.Application.DTOs.Responses.Movies
{
    /// <summary>
    /// Response DTO for movie details
    /// </summary>
    public class MovieResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Amount { get; set; }
        public string PosterPath { get; set; }
        public string Plot { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
        public List<string> Genres { get; set; }
        public decimal Rating { get; set; }
        public int TotalRatings { get; set; }
    }

    /// <summary>
    /// Simplified movie response for lists
    /// </summary>
    public class MovieSummaryResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PosterPath { get; set; }
        public decimal Amount { get; set; }
        public decimal Rating { get; set; }
        public List<string> Genres { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    /// <summary>
    /// Response for movie rating operation
    /// </summary>
    public class MovieRatingResponse
    {
        public long MovieId { get; set; }
        public long UserId { get; set; }
        public int UserRating { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }
}
