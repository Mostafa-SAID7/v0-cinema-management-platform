using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Movies;

namespace MoviesAPI.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("movie");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(e => e.Duration)
                .HasColumnName("duration");
            
            builder.Property(e => e.ReleaseDate)
                .HasColumnName("release_date");
            
            builder.Property(e => e.Amount)
                .HasColumnName("amount")
                .HasColumnType("decimal(18,2)");
            
            builder.Property(e => e.PosterPath)
                .HasColumnName("poster_path")
                .HasMaxLength(500);
            
            builder.Property(e => e.Plot)
                .HasColumnName("plot");
            
            builder.Property(e => e.Actors)
                .HasColumnName("actors");
            
            builder.Property(e => e.Directors)
                .HasColumnName("directors");
            
            builder.Property(e => e.Genres)
                .HasColumnName("genres")
                .HasMaxLength(500);
            
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
                .HasDatabaseName("IX_Movies_IsDeleted");
            
            // Relationships
            builder.HasMany(e => e.Ratings)
                .WithOne(mr => mr.Movie)
                .HasForeignKey(mr => mr.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(e => e.Screenings)
                .WithOne(s => s.Movie)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
