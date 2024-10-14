
namespace Catalog.InfraData.Context
{
    public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
    {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseIdentityByDefaultColumns();

            modelBuilder.ApplyConfiguration(new ProductEttConf());
            modelBuilder.ApplyConfiguration(new ImageEttConf());
            modelBuilder.ApplyConfiguration(new CategoryEttConf());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Database=Catalog;User Id=sa;Password=Password123;", o =>
            {
                o.SetPostgresVersion(new Version(16, 4));
            });
        }
    }
}
