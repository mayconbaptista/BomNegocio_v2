using BuildBlocks.WebApi.Exceptions.Handlers;
using Order.Application;
using Order.Infrastructure;
using Order.WebApi;
using Order.WebApi.Migrations;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApiServices(builder.Configuration);

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    var app = builder.Build();

    // Modo job: `docker compose run --rm <servico> --migrate` aplica as
    // migrations uma unica vez e encerra, antes de subir as replicas novas.
    if (args.Contains("--migrate"))
    {
        await app.ApplyMigrations();
        return;
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Order WebApi - V1");
            options.RoutePrefix = string.Empty;
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            await app.ApplyMigrations();
        }
    }

    app.UseExceptionHandler(options => { });

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}
