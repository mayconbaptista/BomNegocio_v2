using Microsoft.EntityFrameworkCore;

namespace Cart.WebApi.Data
{
    public static class MigrationExtension
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            try
            {

                using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

                var context = serviceScope.ServiceProvider.GetRequiredService<CartContext>();

                await context.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
