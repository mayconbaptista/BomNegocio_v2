using Microsoft.AspNetCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//1.Adicione o Serviço de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()   // Permite o Angular
               .AllowAnyMethod()   // Permite GET, POST, PUT, DELETE, OPTIONS
               .AllowAnyHeader();  // Permite Authorization, etc.
    });
});

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors("CorsPolicy");

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();
