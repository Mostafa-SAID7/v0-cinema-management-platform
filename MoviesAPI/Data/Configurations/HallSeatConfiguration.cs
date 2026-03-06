using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Halls;

namespace MoviesAPI.Data.Configurations
{
    public class HallSeatConfiguration : IEntityTypeConfiguration<HallSeat>
    {
        public void Configure(EntityTypeBuilder<HallSeat> builder)
        {
            builder.ToTable("hall_seats");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.HallId)
                .HasColumnName("hall_id");
            
            builder.Property(e => e.RowNumber)
                .HasColumnName("row_number");
            
            builder.Property(e => e.SeatNumber)
                .HasColumnName("seat_number");
            
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
            builder.HasIndex(e => new { e.HallId, e.RowNumber, e.SeatNumber })
                .IsUnique()
                .HasDatabaseName("IX_HallSeats_HallId_Row_Seat");
            
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_HallSeats_IsDeleted");
            
            // Relationships
            builder.HasMany(e => e.Tickets)
                .WithOne(t => t.HallSeat)
                .HasForeignKey(t => t.HallSeatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
