using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Users;

namespace MoviesAPI.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table is already configured in ApplicationDbContext as "users"
            
            // Identity properties are already configured by IdentityDbContext
            // Id, UserName, NormalizedUserName, Email, NormalizedEmail, 
            // EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, 
            // PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, 
            // LockoutEnd, LockoutEnabled, AccessFailedCount
            
            // Configure custom properties
            builder.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(e => e.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);
            
            builder.Property(e => e.IsActive)
                .HasColumnName("active")
                .HasDefaultValue(true);
            
            builder.Property(e => e.IsAdmin)
                .HasColumnName("is_admin")
                .HasDefaultValue(false);
            
            // Audit properties
            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");
            
            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");
            
            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by");
            
            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by");
            
            // Soft delete properties
            builder.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);
            
            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at");
            
            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by");
            
            // Indexes
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Users_IsDeleted");
            
            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_Users_IsActive");
            
            // Relationships
            builder.HasMany(e => e.MovieRatings)
                .WithOne(mr => mr.User)
                .HasForeignKey(mr => mr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(e => e.Tickets)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
