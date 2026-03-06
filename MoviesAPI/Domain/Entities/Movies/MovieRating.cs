using MoviesAPI.Domain.Common;
using MoviesAPI.Domain.Entities.Users;

namespace MoviesAPI.Domain.Entities.Movies
{
    /// <summary>
    /// MovieRating entity
    /// </summary>
    public class MovieRating : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }    
        public int Rating { get; set; }   
        public string Comment { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
