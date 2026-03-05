namespace MoviesAPI.Application.DTOs.Responses.Screenings
{
    /// <summary>
    /// Response DTO for screening details
    /// </summary>
    public class ScreeningResponse
    {
        public long Id { get; set; }
        public int MovieId { get; set; }
        public MovieInfo Movie { get; set; }
        public int HallId { get; set; }
        public HallInfo Hall { get; set; }
        public DateTime ScreeningDateTime { get; set; }
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }
        public int BookedTickets { get; set; }
        public decimal OccupancyPercentage { get; set; }
        public bool IsAvailable { get; set; }
    }

    /// <summary>
    /// Simplified screening response for lists
    /// </summary>
    public class ScreeningSummaryResponse
    {
        public long Id { get; set; }
        public string MovieName { get; set; }
        public string PosterPath { get; set; }
        public string HallName { get; set; }
        public DateTime ScreeningDateTime { get; set; }
        public int AvailableTickets { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Movie information for screening
    /// </summary>
    public class MovieInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PosterPath { get; set; }
        public int Duration { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Hall information for screening
    /// </summary>
    public class HallInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalSeats { get; set; }
    }

    /// <summary>
    /// Response for seat availability
    /// </summary>
    public class SeatAvailabilityResponse
    {
        public int ScreeningId { get; set; }
        public List<SeatInfo> Seats { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int BookedSeats { get; set; }
    }

    /// <summary>
    /// Seat information
    /// </summary>
    public class SeatInfo
    {
        public int Id { get; set; }
        public int HallSeatId { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public bool IsAvailable { get; set; }
        public long? BookedByUserId { get; set; }
    }
}
