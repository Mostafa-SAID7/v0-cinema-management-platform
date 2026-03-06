namespace MoviesAPI.Application.DTOs.Requests.Movies
{
    /// <summary>
    /// Internal request DTO for creating/updating movies - used by repository layer
    /// </summary>
    public class CreateAndUpdateMovieRequest
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime Release_Date { get; set; }
        public decimal Amount { get; set; }
        public string Poster_Path { get; set; }
        public string Plot { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
        public List<string> Genres { get; set; }
    }
}
