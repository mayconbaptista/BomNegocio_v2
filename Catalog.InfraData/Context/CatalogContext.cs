
using Microsoft.Extensions.Configuration;

namespace Catalog.InfraData.Context
{
    public class CatalogContext(DbContextOptions<CatalogContext> options, IConfiguration configuration) : DbContext(options)
    {
        private readonly IConfiguration _configuration = configuration;
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
            if(!optionsBuilder.IsConfigured)
            {
                string connection = _configuration.GetConnectionString("CatalogConnection") ?? throw new ArgumentNullException("Connection string not found");

                optionsBuilder.UseNpgsql(connection, opt =>
                {
                    opt.SetPostgresVersion(new Version(16, 4));
                    opt.EnableRetryOnFailure(3);
                });
            }
        }
    }
}
