using Microsoft.AspNetCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();

// ref: https://medium.com/c-sharp-programming/ocelot-dotnet-8-hc-serilog-723d342b828a

//var builder = WebHost.CreateDefaultBuilder(args);

//builder.UseKestrel();

//builder.UseContentRoot(Directory.GetCurrentDirectory());

//builder.ConfigureAppConfiguration((hostingContext, config) =>
//{
//    //config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
//    config.AddJsonFile("launchSettings.json", optional: true, reloadOnChange: true);
//    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
//    config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//    config.AddEnvironmentVariables();
//});

//builder.ConfigureServices(services =>
//{
//    services.AddOcelot();
//});

//builder.UseIISIntegration();

//builder.Configure(async app => await app.UseOcelot());

//var app = builder.Build();

//await app.RunAsync();