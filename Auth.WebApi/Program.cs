using Auth.WebApi;
using BuildBlocks.WebApi.Exceptions.Handlers;
using BuildBlocks.WebApi.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    InjectionContainer.RegisterServices(builder.Services, builder.Configuration);

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(typeof(Program).Assembly);

    // DICA: Auth deve vir antes do Identity em alguns cenários, mas aqui está ok
    builder.Services.AddAuthorization(builder.Configuration);
    builder.Services.AddAuthentication(builder.Configuration);

    builder.Services.AddIdentity<UserModel, IdentityRole>()
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

    var connection = builder.Configuration.GetConnectionString("AuthConnection")
        ?? throw new ArgumentNullException("Connection string not found");

    builder.Services.AddDbContext<AuthDbContext>((sp, options) =>
    {
        options.UseNpgsql(connection, opt =>
        {
            opt.SetPostgresVersion(new Version(16, 4));
            opt.EnableRetryOnFailure(3);
            opt.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName!);
        });
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    });

    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    var app = builder.Build();

    if (!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth WebApi - V1");
            options.RoutePrefix = string.Empty;
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.ApplyMigrations();
        }
    }

    app.UseExceptionHandler(options => { });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);

    throw;
}