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
                .HasColumnName("create_at");

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

            builder.ComplexProperty(e => e.Payment, a =>
            {
                a.Property(e => e.Currency)
                    .HasColumnName("payment_currency")
                    .HasColumnType("varchar(3)")
                    .IsRequired(true);

                a.Property(e => e.CardCvv)
                    .HasColumnName("payment_cardcvv")
                    .HasMaxLength(3)
                    .IsRequired(false);

                a.Property(e => e.CardHolderName)
                    .HasColumnName("payment_cardHolderName")
                    .HasMaxLength(60)
                    .IsRequired(false);

                a.Property(e => e.CardNumber)
                    .HasColumnName("payment_cardNumber")
                    .HasMaxLength(16)
                    .IsRequired(false);

                a.Property(e => e.CardExpirationDate)
                    .HasColumnName("payment_cardExpirationDate")
                    .HasMaxLength(60)
                    .IsRequired();

                a.Property(e => e.QrCodePayload)
                    .HasColumnName("payment_qrCodePayload")
                    .HasMaxLength(60)
                    .IsRequired();
            });

            builder.ComplexProperty(e => e.Delivery, a =>
            {
                a.Property(e => e.Type)
                    .HasColumnName("delivery_type")
                    .IsRequired();

                a.Property(e => e.EstimatedDeliveryDate)
                    .HasColumnName("delivery_estimatedDeliveryDate")
                    .IsRequired();

                a.Property(e => e.Price)
                    .HasColumnName("delivery_price")
                    .IsRequired();
            });

            builder.HasMany(e => e.OrderItems)
                .WithOne()
                .HasForeignKey(e => e.OrderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Order_OrderItem");
        }
    }
}
