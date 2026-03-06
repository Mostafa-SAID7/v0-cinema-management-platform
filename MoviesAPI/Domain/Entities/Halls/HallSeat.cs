using MoviesAPI.Domain.Common;
using MoviesAPI.Domain.Entities.Tickets;

namespace MoviesAPI.Domain.Entities.Halls
{
    /// <summary>
    /// HallSeat entity
    /// </summary>
    public class HallSeat : BaseEntity
    {
        public Guid HallId { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        
        // Navigation properties
        public virtual Hall Hall { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
