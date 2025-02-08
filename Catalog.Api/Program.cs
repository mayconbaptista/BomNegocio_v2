using Carter;
using Catalog.Api.Services;
using BuildBlocks.WebApi.Extensions;
using BuildBlocks.WebApi.Behaviors;
using Catalog.Api;
using Catalog.Api.Data.Extensions;
using Catalog.Api.Dtos;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddCarter();

    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
    });

    builder.Services.AddGrpc();

    // configurando o container de DI
    builder.Services.AddApiServices(builder.Configuration);
    MapsterConfig.RegisterMappings();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(typeof(Program).Assembly);

    var app = builder.Build();

    if(!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart.Api - V1");
            options.RoutePrefix = string.Empty;
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            await app.ApplyMigrations();
        }
    }

    // Configure the HTTP request pipeline.
    app.MapGrpcService<GreeterService>();
    app.MapGrpcService<ProductService>();

    app.MapCarter();
    app.Run();
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

