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
}
