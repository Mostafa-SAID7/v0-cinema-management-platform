using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Movies;

namespace MoviesAPI.Data.Configurations
{
    public class MovieRatingConfiguration : IEntityTypeConfiguration<MovieRating>
    {
        public void Configure(EntityTypeBuilder<MovieRating> builder)
        {
            builder.ToTable("movie_ratings");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.UserId)
                .HasColumnName("user_id");
            
            builder.Property(e => e.MovieId)
                .HasColumnName("movie_id");
            
            builder.Property(e => e.Rating)
                .HasColumnName("rating");
            
            builder.Property(e => e.Comment)
                .HasColumnName("comment")
                .HasMaxLength(1000);
            
            // BaseEntity properties
            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");
            
            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");
            
            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by");
            
            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by");
            
            builder.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);
            
            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");
            
            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by");
            
            // Indexes
            builder.HasIndex(e => new { e.UserId, e.MovieId })
                .IsUnique()
                .HasDatabaseName("IX_MovieRatings_UserId_MovieId");
            
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_MovieRatings_IsDeleted");
            
            // Relationships configured in User and Movie configurations
        }
    }
}
