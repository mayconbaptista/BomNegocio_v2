using Microsoft.EntityFrameworkCore;

namespace Auth.WebApi.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            using AuthDbContext context = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
            context.Database.Migrate();
        }
    }
}
