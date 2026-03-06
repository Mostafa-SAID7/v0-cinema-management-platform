namespace MoviesAPI.Application.DTOs.Requests.Tickets
{
    /// <summary>
    /// Internal request DTO for creating tickets - used by repository layer
    /// </summary>
    public class CreateTicketInternalRequest
    {
        public long Movie_Id { get; set; }
        public long User_Id { get; set; }
        public DateTime Watch_Movie { get; set; }
        public decimal Price { get; set; }
        public int hall_seat_id { get; set; }
    }
}
