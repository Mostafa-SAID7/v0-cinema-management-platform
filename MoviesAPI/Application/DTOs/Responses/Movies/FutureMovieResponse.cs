namespace MoviesAPI.Application.DTOs.Responses.Movies
{
    /// <summary>
    /// Response DTO for future movies
    /// </summary>
    public class FutureMovieResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genres { get; set; }
        public string Poster_Path { get; set; }
    }
}
