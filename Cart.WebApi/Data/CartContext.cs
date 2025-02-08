using Cart.WebApi.Data.Configuration;
using Cart.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cart.WebApi.Data
{
    public class CartContext(DbContextOptions<CartContext> options) : DbContext(options)
    {
        public DbSet<CartEntity> Carts { get; set; } = null!;
        public DbSet<CartItemEntity> CartItems { get; set; } = null!;

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfiguration(new CartEntityConfiguration());
        //    modelBuilder.ApplyConfiguration(new CartItemEntityConfiguration());
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
