
using BuildBlocks.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Domain.ValueObjects;

namespace Order.Infrastructure.Data
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options)
            : base(options)
        {
        }

        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
        public DbSet<Address> Addresses => Set<Address>();
    }
}
