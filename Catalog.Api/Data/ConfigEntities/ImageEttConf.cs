
using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.Data.ConfigEntities
{
    public sealed class ImageEttConf : IEntityTypeConfiguration<ImageEntity>
    {
        public void Configure(EntityTypeBuilder<ImageEntity> builder)
        {
            builder.ToTable("imagem");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.path)
                .HasColumnName("path_url")
                .HasMaxLength(200)
                .IsRequired(true);

            builder.Property(p => p.name)
                .HasColumnName("file_name")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(p => p.ProductId)
                .HasColumnName("produto_Id")
                .IsRequired(true);

            builder.HasOne(p => p.Product)
                .WithMany(c => c.Images)
                .HasForeignKey(p => p.ProductId)
                .HasConstraintName("FK_imagem_produto")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.ProductId)
                .IsUnique(true)
                .HasDatabaseName("IX_categoria_nome");
        }
    }
}
