using Classifields.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifields.Infra.Data.EntitiesConfiguration
{
    public class WisheConfiguration : IEntityTypeConfiguration<WisheEntity>
    {
        public void Configure(EntityTypeBuilder<WisheEntity> builder)
        {
            builder.ToTable("WISHE");
            builder.Ignore(x => x.Id);

            builder.HasKey(x => new { x.ClientId, x.AnnouncementId });


            builder.HasOne(x => x.Client)
                .WithMany(x => x.Wishes)
                .HasForeignKey(x => x.ClientId)
                .HasConstraintName("FK_WISHE_CLIENT")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Announcement)
                .WithMany(x => x.Wishes)
                .HasForeignKey(x => x.AnnouncementId)
                .HasConstraintName("FK_WISHE_ANNOUNCEMENT")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.CreationDate)
                .HasColumnName("CREATION_DATE")
                .HasColumnType("date")
                .IsRequired(true);
        }
    }
}
