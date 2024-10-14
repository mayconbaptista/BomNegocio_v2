using Classifields.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifields.Infra.Data.EntitiesConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("CATEGORY");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("ID")
                .IsRequired(true)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.ParentCategoryId)
                .HasColumnName("PARENT_CATEGORY_ID")
                .IsRequired(false)
                .HasDefaultValue(null);

            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .HasConstraintName("FK_PARENT_CATEGORY")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Name)
                .HasColumnName("NOME")
                .IsRequired(true);

            builder.Property(x => x.Description)
                .HasColumnName("DESCRIPTION")
                .IsRequired(true);
        }
    }
}
