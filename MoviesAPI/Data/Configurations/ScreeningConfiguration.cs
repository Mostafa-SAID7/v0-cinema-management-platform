using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Screenings;

namespace MoviesAPI.Data.Configurations
{
    public class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
    {
        public void Configure(EntityTypeBuilder<Screening> builder)
        {
            builder.ToTable("screenings");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.MovieId)
                .HasColumnName("movie_id");
            
            builder.Property(e => e.ScreeningDateTime)
                .HasColumnName("screening_date_time");
            
            builder.Property(e => e.TotalTickets)
                .HasColumnName("total_tickets");
            
            builder.Property(e => e.AvailableTickets)
                .HasColumnName("available_tickets");
            
            builder.Property(e => e.HallId)
                .HasColumnName("hall_id");
            
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
            builder.HasIndex(e => e.ScreeningDateTime)
                .HasDatabaseName("IX_Screenings_DateTime");
            
            builder.HasIndex(e => new { e.MovieId, e.ScreeningDateTime })
                .HasDatabaseName("IX_Screenings_Movie_DateTime");
            
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Screenings_IsDeleted");
            
            // Relationships
            builder.HasMany(e => e.Tickets)
                .WithOne()
                .HasForeignKey(t => t.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
