using Classifields.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifields.Infra.Data.EntitiesConfiguration
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<AnnouncementEntity>
    {
        public void Configure(EntityTypeBuilder<AnnouncementEntity> builder)
        {
            builder.ToTable("ANNOUNCEMENT");
            builder.HasKey(a => a.Id);

            builder.Property<uint>(a => a.Id)
                .HasColumnName("ID")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.HasOne(a => a.Advertiser)
                .WithMany(a => a.Announcements)
                .HasForeignKey(a => a.AdvertiserId)
                .HasConstraintName("FK_ANNOUNCEMENT_ADVERTISER")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Category)
                .WithMany(a => a.Announcements)
                .HasForeignKey(a => a.CategoryId)
                .HasConstraintName("FK_ANNOUNCEMENT_CATEGORY")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Address)
                .WithOne(a => a.Announcement); // faltando algo?

            builder.Property(x => x.Title)
                .HasColumnName("TITLE")
                .HasMaxLength(60)
                .IsRequired(true);

            builder.Property(x => x.Description)
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(500)
                .IsRequired(true);

            builder.Property(x => x.Price)
                .HasColumnName("PRICE")
                .HasColumnType("decimal")
                .IsRequired(true);

            //builder.Property(x => x.CreationDate)
            //    .HasColumnName("CREATION_DATE")
            //    .HasColumnType("date")
            //    .IsRequired(true);

            //builder.Property(x => x.DeactivationDate)
            //    .HasColumnName("DEACTIVATION_DATE")
            //    .HasColumnType("date")
            //    .IsRequired(false);
        }
    }
}
