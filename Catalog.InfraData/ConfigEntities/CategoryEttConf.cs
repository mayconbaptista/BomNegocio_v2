﻿using Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.InfraData.ConfigEntities
{
    public sealed class CategoryEttConf : IEntityTypeConfiguration<CategoryModel>
    {
        public void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            builder.ToTable("categoria");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(p => p.Description)
                .HasColumnName("descricao")
                .HasMaxLength(500).IsRequired(true);

            builder.Property(p => p.ParentCategoryId)
                .HasColumnName("categoria_pai")
                .IsRequired(false);

            builder.HasOne(p => p.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(p => p.ParentCategoryId)
                .HasConstraintName("FK_categoria_categoriaPai")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}