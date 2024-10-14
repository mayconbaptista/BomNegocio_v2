
namespace Catalog.InfraData.ConfigEntities
{
    public sealed class ImageEttConf : IEntityTypeConfiguration<ImageModel>
    {
        public void Configure(EntityTypeBuilder<ImageModel> builder)
        {
            builder.ToTable("imagem");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Path)
                .HasColumnName("path_url")
                .HasMaxLength(200)
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
