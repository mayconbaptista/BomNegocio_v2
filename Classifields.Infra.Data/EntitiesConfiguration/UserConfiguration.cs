using Classifields.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifields.Infra.Data.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("USER");
            builder.HasKey(x => x.Id);
            builder.Property(u => u.Id)
                .HasColumnName("ID")
                .IsRequired(true)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnName("NAME")
                .HasMaxLength(60)
                .IsRequired(true);

            builder.Property(x => x.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(x => x.Cpf)
                .HasColumnName("CPF")
                .HasMaxLength(11)
                .IsRequired(true);
        }
    }
}
