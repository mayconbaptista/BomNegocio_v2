
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Data.Configurations
{
    internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("order_item");
            builder.HasKey(e => new {e.OrderId, e.ProductId});

            builder.Ignore(e => e.Id);

            builder.Property(e => e.OrderId)
                .HasColumnName("order_id")
                .IsRequired(true);

            builder.Property(e => e.ProductId)
                .HasColumnName("product_id")
                .IsRequired(true);

            builder.Property(e => e.UnitPrice)
                .HasColumnName("unit_price")
                .IsRequired(true);

            builder.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired(true);

            builder.HasOne<OrderEntity>()
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
