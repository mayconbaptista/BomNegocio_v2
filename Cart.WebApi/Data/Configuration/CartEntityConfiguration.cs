using Cart.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.WebApi.Data.Configuration
{
    internal class CartEntityConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.ToTable("Cart");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired(true);


            builder.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(e => e.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
