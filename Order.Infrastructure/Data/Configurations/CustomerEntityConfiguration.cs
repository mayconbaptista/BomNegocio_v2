
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Data.Configurations
{
    internal class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired(true);

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(100)
                .IsRequired(true);
        }
    }
}
