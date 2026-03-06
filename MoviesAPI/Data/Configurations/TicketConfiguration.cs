using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Tickets;

namespace MoviesAPI.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("tickets");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.MovieId)
                .HasColumnName("movie_id");
            
            builder.Property(e => e.UserId)
                .HasColumnName("user_id");
            
            builder.Property(e => e.WatchDateTime)
                .HasColumnName("watch_movie");
            
            builder.Property(e => e.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(18,2)");
            
            builder.Property(e => e.HallSeatId)
                .HasColumnName("hall_seat_id");
            
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
            builder.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_Tickets_UserId");
            
            builder.HasIndex(e => e.WatchDateTime)
                .HasDatabaseName("IX_Tickets_WatchDateTime");
            
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Tickets_IsDeleted");
            
            // Relationships
            builder.HasOne(e => e.Movie)
                .WithMany()
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
