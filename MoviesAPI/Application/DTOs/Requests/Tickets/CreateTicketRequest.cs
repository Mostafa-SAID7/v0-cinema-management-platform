namespace MoviesAPI.Application.DTOs.Requests.Tickets
{
    /// <summary>
    /// Request DTO for creating a new ticket
    /// </summary>
    public class CreateTicketRequest
    {
        public long MovieId { get; set; }
        public long UserId { get; set; }
        public DateTime WatchDateTime { get; set; }
        public decimal Price { get; set; }
        public int HallSeatId { get; set; }
    }

    /// <summary>
    /// Request DTO for booking multiple seats
    /// </summary>
    public class BookSeatsRequest
    {
        public string Username { get; set; }
        public int ScreeningId { get; set; }
        public List<int> SelectedSeatIds { get; set; }
    }

    /// <summary>
    /// Request DTO for getting user tickets with filters
    /// </summary>
    public class GetUserTicketsRequest
    {
        public long UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IncludePastTickets { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Request DTO for canceling a ticket
    /// </summary>
    public class CancelTicketRequest
    {
        public long TicketId { get; set; }
        public long UserId { get; set; }
        public string Reason { get; set; }
    }
}
