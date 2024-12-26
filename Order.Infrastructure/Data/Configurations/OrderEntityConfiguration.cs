using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Data.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired(true)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Status)
                .HasColumnName("status_code")
                .IsRequired(true);

            builder.Property(e => e.CreateAt)
                .HasColumnName("created_at");

            builder.ComplexProperty(e => e.ShippingAddress, a =>
            {
                a.Property(e => e.Name)
                    .HasColumnName("shipping_address_name")
                    .IsRequired();

                a.Property(e => e.City)
                    .HasColumnName("shipping_address_city")
                    .IsRequired();
                a.Property(e => e.Country)
                    .HasColumnName("shipping_address_country")
                    .IsRequired();

                a.Property(e => e.State)
                    .HasColumnName("shipping_address_state")
                    .IsRequired();

                a.Property(e => e.Street)
                    .HasColumnName("shipping_address_street")
                    .IsRequired();

                a.Property(e => e.ZipCode)
                .HasColumnName("shipping_address_zip_code")
                    .IsRequired();
            });


            builder.ComplexProperty(e => e.BillingAddress, a =>
            {
                a.Property(e => e.Name)
                    .HasColumnName("billing_address_name")
                    .HasMaxLength(60)
                    .IsRequired();

                a.Property(e => e.City)
                    .HasColumnName("billing_address_city")
                    .HasMaxLength(60)
                    .IsRequired();

                a.Property(e => e.Country)
                    .HasColumnName("billing_address_country")
                    .HasMaxLength(60)
                    .IsRequired();

                a.Property(e => e.State)
                    .HasColumnName("billing_address_state")
                    .HasMaxLength(60)
                    .IsRequired();

                a.Property(e => e.Street)
                    .HasColumnName("billing_address_street")
                    .HasMaxLength(60)
                    .IsRequired();

                a.Property(e => e.ZipCode)
                    .HasColumnName("billing_address_zip_code")
                    .HasMaxLength(60)
                    .IsRequired();
            });

            builder.HasOne<CustomerEntity>()
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Order_Customer");

            builder.HasMany(e => e.OrderItems)
                .WithOne()
                .HasForeignKey(e => e.OrderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Order_OrderItem");
        }
    }
}
