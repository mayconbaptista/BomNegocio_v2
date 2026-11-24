using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data.Configurations
{
    internal class PaymentEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.PaymentEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.ToTable("Payment");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired(true);

            builder.Property(x => x.PxId)
                .HasColumnName("px_id")
                .IsRequired(true);

            builder.Property(x => x.Key)
                .HasColumnName("key")
                .IsRequired(true);

            builder.Property(x => x.PaymentMethod)
                .HasColumnName("method")
                .HasConversion<string>()
                .IsRequired(true);

            builder.Property(x => x.ExpireIn)
                .HasColumnName("expire_in")
                .IsRequired(true);

            builder.Property(x => x.PixCopyAndPaste)
                .HasColumnName("pix_copy_and_paste")
                .IsRequired(true);

            builder.Property(x => x.Location)
                .HasColumnName("location")
                .IsRequired(true);
        }
    }
}
