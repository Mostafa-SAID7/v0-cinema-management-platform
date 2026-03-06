namespace MoviesAPI.Application.DTOs.Responses.Movies
{
    /// <summary>
    /// Response DTO for movie summary in lists
    /// </summary>
    public class MovieSummaryResponse
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public string PosterUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double? AverageRating { get; set; }
    }
}
