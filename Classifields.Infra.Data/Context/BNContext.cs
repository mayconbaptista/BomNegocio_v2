using Classifields.Domain.Entities;
using Classifields.Infra.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Classifields.Infra.Data.Context
{
    public class BNContext : IdentityDbContext<ApplicationUser>
    {
        public BNContext(DbContextOptions<BNContext> options) : base(options) { }

        public DbSet<AdvertiserEntity> Advertisers { get; set; }
        public DbSet<AnnouncementEntity> Announcements { get; set; }
        public DbSet<EvaluetionEntity> Evaluetions { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<WisheEntity> Wishes { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<ImageEntity> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new AdvertiserConfiguration());
            builder.ApplyConfiguration(new AnnouncementConfiguration());
            builder.ApplyConfiguration(new EvaluetionConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ClientConfiguration());
            builder.ApplyConfiguration(new WisheConfiguration());
            builder.ApplyConfiguration(new AddressConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
