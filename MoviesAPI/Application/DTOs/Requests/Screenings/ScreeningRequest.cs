namespace MoviesAPI.Application.DTOs.Requests.Screenings
{
    /// <summary>
    /// Request DTO for creating a new screening
    /// </summary>
    public class CreateScreeningRequest
    {
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime ScreeningDateTime { get; set; }
    }

    /// <summary>
    /// Request DTO for updating a screening
    /// </summary>
    public class UpdateScreeningRequest
    {
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime ScreeningDateTime { get; set; }
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }
    }

    /// <summary>
    /// Request DTO for getting screenings with filters
    /// </summary>
    public class GetScreeningsRequest
    {
        public int? MovieId { get; set; }
        public int? HallId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool OnlyAvailable { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
