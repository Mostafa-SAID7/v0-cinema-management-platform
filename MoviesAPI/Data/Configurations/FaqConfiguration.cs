using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.Domain.Entities.Faqs;

namespace MoviesAPI.Data.Configurations
{
    public class FaqConfiguration : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.ToTable("faqs");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.Question)
                .HasColumnName("question")
                .IsRequired()
                .HasMaxLength(500);
            
            builder.Property(e => e.Answer)
                .HasColumnName("answer")
                .IsRequired();
            
            builder.Property(e => e.Category)
                .HasColumnName("category")
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
            builder.HasIndex(e => e.Category)
                .HasDatabaseName("IX_Faqs_Category");
            
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Faqs_IsDeleted");
        }
    }
}
