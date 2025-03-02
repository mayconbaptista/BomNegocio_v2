using Cart.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.WebApi.Data.Configuration
{
    public class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItemEntity>
    {
        public void Configure(EntityTypeBuilder<CartItemEntity> builder)
        {
            builder.ToTable("Cart_Item");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ProductId)
                .HasColumnName("product_id")
                .IsRequired(true);

            builder.Property(e => e.CartId)
                .HasColumnName("cart_id")
                .IsRequired(true);

            builder.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired(true);

            builder.HasOne<CartEntity>()
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.CartId, e.ProductId });
        }
    }
}
