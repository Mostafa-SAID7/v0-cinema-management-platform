using MoviesAPI.Domain.Common;
using MoviesAPI.Domain.Entities.Movies;
using MoviesAPI.Domain.Entities.Halls;
using MoviesAPI.Domain.Entities.Tickets;

namespace MoviesAPI.Domain.Entities.Screenings
{
    /// <summary>
    /// Screening entity - Aggregate Root
    /// </summary>
    public class Screening : BaseEntity
    {
        public Guid MovieId { get; set; }
        public DateTime ScreeningDateTime { get; set; }
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }
        public Guid HallId { get; set; }
        
        // Navigation properties
        public virtual Movie Movie { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
