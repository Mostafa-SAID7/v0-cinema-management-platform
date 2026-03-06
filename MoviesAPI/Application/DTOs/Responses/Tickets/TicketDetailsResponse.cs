namespace MoviesAPI.Application.DTOs.Responses.Tickets
{
    /// <summary>
    /// Response DTO for detailed ticket information with joined data
    /// </summary>
    public class TicketDetailsResponse
    {
        public long Id { get; set; }
        public string MovieName { get; set; }
        public string UserName { get; set; }
        public string PosterPath { get; set; }
        public DateTime Watch_Movie { get; set; }
        public decimal Price { get; set; }
        public string HallName { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
