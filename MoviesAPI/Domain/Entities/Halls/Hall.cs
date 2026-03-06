using MoviesAPI.Domain.Common;
using MoviesAPI.Domain.Entities.Screenings;

namespace MoviesAPI.Domain.Entities.Halls
{
    /// <summary>
    /// Hall entity - Aggregate Root
    /// </summary>
    public class Hall : BaseEntity
    {
        public string Name { get; set; }
        
        // Navigation properties
        public virtual ICollection<HallSeat> HallSeats { get; set; }
        public virtual ICollection<Screening> Screenings { get; set; }
    }
}
