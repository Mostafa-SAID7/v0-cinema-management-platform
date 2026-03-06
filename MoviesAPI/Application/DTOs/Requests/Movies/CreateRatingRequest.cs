namespace MoviesAPI.Application.DTOs.Requests.Movies
{
    /// <summary>
    /// Request DTO for creating movie ratings
    /// </summary>
    public class CreateRatingRequest
    {
        public long MovieId { get; set; }
        public long UserId { get; set; }
        public int Rating { get; set; }
    }
}
