using Auth.WebApi;
using BuildBlocks.WebApi.Exceptions.Handlers;
using BuildBlocks.WebApi.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();

        });
    });

    // Add services to the container.
    InjectionContainer.RegisterServices(builder.Services, builder.Configuration);

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(typeof(Program).Assembly);

    AuthorizationExtension.AddAuthorization(builder.Services, builder.Configuration);
    AuthenticationExtension.AddAuthentication(builder.Services, builder.Configuration);

    builder.Services.AddIdentity<UserModel, IdentityRole>()
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

    var connection = builder.Configuration.GetConnectionString("AuthConnection") 
        ?? throw new ArgumentNullException("Connection string not found");

    builder.Services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseNpgsql(connection, opt =>
        {
            opt.SetPostgresVersion(new Version(16, 4));
            opt.EnableRetryOnFailure(3);
        });
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
        options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
    });

    // responsabilidades transversais
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth WebApi - V1");
            options.RoutePrefix = string.Empty;
        });

        if(app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.ApplyMigrations();
        }
    }

    app.UseCors();

    app.UseExceptionHandler(options => { });

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}

