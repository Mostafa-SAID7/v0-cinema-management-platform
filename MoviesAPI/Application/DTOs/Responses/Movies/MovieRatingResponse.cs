namespace MoviesAPI.Application.DTOs.Responses.Movies
{
    /// <summary>
    /// Response DTO for movie ratings
    /// </summary>
    public class MovieRatingResponse
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long MovieId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
