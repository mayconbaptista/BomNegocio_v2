using Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.InfraData.ConfigEntities
{
    public sealed class ProductEttConf : IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            builder.ToTable("produto");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.SkuCode)
                .HasColumnName("sku")
                .HasMaxLength(20)
                .IsRequired(true);

            builder.Property(p => p.Name)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(p => p.Description)
                .HasColumnName("descricao")
                .HasMaxLength(200)
                .IsRequired(true);

            builder.Property(p => p.Price)
                .HasColumnName("preco")
                .HasColumnType("decimal(10,2)")
                .IsRequired(true);

            builder.Property(p => p.Amount)
                .HasColumnName("quantidade")
                .IsRequired(true);

            builder.Property(p => p.CategoryId)
                .HasColumnName("categoria_id")
                .IsRequired(true);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .HasConstraintName("FK_produto_categoria")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.SkuCode)
                .IsUnique(true)
                .HasDatabaseName("idx_produto_sku");
        }
    }
}
