namespace MoviesAPI.Application.DTOs.Requests.Movies
{
    /// <summary>
    /// Request DTO for creating future movies
    /// </summary>
    public class CreateFutureMovieRequest
    {
        public string Name { get; set; }
        public string Genres { get; set; }
        public string Poster_Path { get; set; }
    }
}
