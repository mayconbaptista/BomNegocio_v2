using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;

namespace Order.WebApi.Migrations
{
    public static class MigrationExtension
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<OrderContext>();
            
            await context.Database.MigrateAsync();
        }
    }
}
