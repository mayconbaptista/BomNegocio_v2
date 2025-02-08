using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Data
{
    public class CatalogContext(DbContextOptions<CatalogContext> options, IConfiguration configuration) : DbContext(options)
    {
        private readonly IConfiguration _configuration = configuration;
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly!);

            base.OnModelCreating(modelBuilder);
        }
    }
}
