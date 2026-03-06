using MoviesAPI.Domain.Common;
using MoviesAPI.Domain.Entities.Screenings;

namespace MoviesAPI.Domain.Entities.Movies
{
    /// <summary>
    /// Movie entity - Aggregate Root
    /// </summary>
    public class Movie : BaseEntity
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Amount { get; set; } 
        public string PosterPath { get; set; }  
        public string Plot { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
        public string Genres { get; set; }
        public decimal Rating { get; set; } // Weighted average rating
        
        // Navigation properties
        public virtual ICollection<MovieRating> Ratings { get; set; }
        public virtual ICollection<Screening> Screenings { get; set; }
    }
}
