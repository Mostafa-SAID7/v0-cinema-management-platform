namespace MoviesAPI.Domain.Common
{
    /// <summary>
    /// Base entity with audit and soft delete properties
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Unique identifier for the entity
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the entity was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID (Guid) who created the entity
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// User ID (Guid) who last updated the entity
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag - when true, entity is considered deleted
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Date and time when the entity was soft deleted
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// User ID (Guid) who soft deleted the entity
        /// </summary>
        public Guid? DeletedBy { get; set; }

        /// <summary>
        /// Marks the entity as deleted (soft delete)
        /// </summary>
        /// <param name="deletedBy">User ID who is deleting the entity</param>
        public void SoftDelete(Guid? deletedBy = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        /// <summary>
        /// Restores a soft deleted entity
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
