using Classifields.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifields.Infra.Data.EntitiesConfiguration
{
    public class AdvertiserConfiguration : IEntityTypeConfiguration<AdvertiserEntity>
    {
        public void Configure(EntityTypeBuilder<AdvertiserEntity> builder)
        {
            builder.ToTable("ADVERTISER");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ID")
                .IsRequired(true)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
                .HasColumnName("USER_ID")
                .IsRequired(true);

            builder.HasOne(a => a.User)
                .WithOne(z => z.Advertiser)
                .HasForeignKey<AdvertiserEntity>(a => a.UserId)
                .HasConstraintName("FK_ADVERTISER_USER")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(x => x.DeactivationDate)
            //    .HasColumnName("DEACTIVATION_DATE")
            //    .HasColumnType("date")
            //    .IsRequired(false)
            //    .HasDefaultValue(null);

            //builder.Property(x => x.CreationDate)
            //    .HasColumnName("CREATION_DATE")
            //    .HasColumnType("date")
            //    .IsRequired(true);
        }
    }
}
