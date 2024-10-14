using Swashbuckle.AspNetCore.SwaggerGen;

namespace Catalog.WebApi.ExtensionsConfig
{
    public static class SwaggerExtensionConfig
    {
        public static void AddSwagger(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Catalog API",
                Version = "v1",
                Description = "A simple API to manage products and categories",
                Contact = new OpenApiContact
                {
                    Name = "Maycon Douglas Batista dos Santos",
                    Email = "mayconbaptista01@outlook.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        }
    }
}
