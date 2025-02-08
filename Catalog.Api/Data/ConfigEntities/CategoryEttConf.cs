using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.Data.ConfigEntities
{
    public sealed class CategoryEttConf : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categoria");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(p => p.Description)
                .HasColumnName("descricao")
                .HasMaxLength(500)
                .IsRequired(true);

            builder.Property(p => p.ParentCategoryId)
                .HasColumnName("categoria_pai")
                .IsRequired(false);

            builder.HasOne(p => p.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(p => p.ParentCategoryId)
                .HasConstraintName("FK_categoria_categoriaPai")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
