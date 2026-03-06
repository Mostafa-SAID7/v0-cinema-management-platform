using MoviesAPI.Domain.Common;
using MoviesAPI.Domain.Entities.Movies;
using MoviesAPI.Domain.Entities.Users;
using MoviesAPI.Domain.Entities.Halls;
using MoviesAPI.Domain.Enums;

namespace MoviesAPI.Domain.Entities.Tickets
{
    /// <summary>
    /// Ticket entity - Aggregate Root
    /// </summary>
    public class Ticket : BaseEntity
    {
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public DateTime WatchDateTime { get; set; }
        public decimal Price { get; set; }
        public Guid HallSeatId { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Active;
        
        // Navigation properties
        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }
        public virtual HallSeat HallSeat { get; set; }
    }
}
