using Microsoft.AspNetCore.Identity;
using MoviesAPI.Domain.Common;
using MoviesAPI.Domain.Entities.Movies;
using MoviesAPI.Domain.Entities.Tickets;

namespace MoviesAPI.Domain.Entities.Users
{
    /// <summary>
    /// User entity - Aggregate Root with ASP.NET Core Identity
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        
        // Audit properties from BaseEntity
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        
        // Soft delete properties
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

        // Computed property
        public bool IsAdmin { get; set; }
        
        // Navigation properties
        public virtual ICollection<MovieRating> MovieRatings { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        
        /// <summary>
        /// Marks the user as deleted (soft delete)
        /// </summary>
        public void SoftDelete(Guid? deletedBy = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
            IsActive = false;
        }

        /// <summary>
        /// Restores a soft deleted user
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
