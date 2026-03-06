using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Halls;

namespace MoviesAPI.Data.Configurations
{
    public class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {
            builder.ToTable("halls");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);
            
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
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Halls_IsDeleted");
            
            // Relationships
            builder.HasMany(e => e.HallSeats)
                .WithOne(hs => hs.Hall)
                .HasForeignKey(hs => hs.HallId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(e => e.Screenings)
                .WithOne(s => s.Hall)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
