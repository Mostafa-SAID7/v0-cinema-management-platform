namespace MoviesAPI.Application.DTOs.Responses.Tickets
{
    /// <summary>
    /// Response DTO for ticket details
    /// </summary>
    public class TicketResponse
    {
        public long Id { get; set; }
        public long MovieId { get; set; }
        public string MovieName { get; set; }
        public string PosterPath { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public DateTime WatchDateTime { get; set; }
        public decimal Price { get; set; }
        public string HallName { get; set; }
        public int Row { get; set; }
        public int SeatNumber { get; set; }
        public string Status { get; set; } // Active, Cancelled, Used
        public DateTime BookedAt { get; set; }
    }

    /// <summary>
    /// Simplified ticket response for lists
    /// </summary>
    public class TicketSummaryResponse
    {
        public long Id { get; set; }
        public string MovieName { get; set; }
        public string PosterPath { get; set; }
        public DateTime WatchDateTime { get; set; }
        public decimal Price { get; set; }
        public string HallName { get; set; }
        public int Row { get; set; }
        public int SeatNumber { get; set; }
    }

    /// <summary>
    /// Response for booking operation
    /// </summary>
    public class BookingResponse
    {
        public List<long> TicketIds { get; set; }
        public int TotalSeats { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingReference { get; set; }
        public DateTime BookedAt { get; set; }
    }
}
