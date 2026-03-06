namespace MoviesAPI.Application.DTOs.Responses.Halls
{
    /// <summary>
    /// Response DTO for hall details with seats
    /// </summary>
    public class HallResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Seats_Per_Row { get; set; }
        public List<HallSeatResponse> Seats { get; set; } = new List<HallSeatResponse>();
    }

    /// <summary>
    /// Response DTO for hall seat information
    /// </summary>
    public class HallSeatResponse
    {
        public int SeatId { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
    }
}
