namespace MoviesAPI.Application.DTOs.Requests.Screenings
{
    /// <summary>
    /// Request DTO for booking seats in a screening
    /// </summary>
    public class BookSeatsRequest
    {
        public string username { get; set; }
        public List<int> SelectedSeatsId { get; set; }
    }
}
