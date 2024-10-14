using Classifields.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifields.Infra.Data.EntitiesConfiguration
{
    public class AddressConfiguration : IEntityTypeConfiguration<AddressEntity>
    {
        public void Configure(EntityTypeBuilder<AddressEntity> builder)
        {
            builder.ToTable("ADDRESS");
            builder.HasKey(e => e.Id);

            builder.Property(x => x.UserId)
                .HasColumnName("USER_ID")
                .IsRequired(false)
                .HasDefaultValue(null);

            builder.Property(x => x.AnnouncementId)
                .HasColumnName("ANNOUNCEMENT_ID")
                .IsRequired(false)
                .HasDefaultValue(null);

            builder.HasOne(e => e.User)
                .WithMany(e => e.Addresses)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_ADDRESS_USER")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(e => e.Announcement)
            //    .WithOne(e => e.Address)
            //    .HasForeignKey(e => e.)
            //    .HasConstraintName("FK_ADDRESS_ANNOUNCEMENT")
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
