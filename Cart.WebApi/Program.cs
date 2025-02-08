
using BuildBlocks.WebApi.Behaviors;
using BuildInBlocks.Messaging.Extensions;
using Cart.WebApi;

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

    builder.Services.AddCarter();

    // Add services to the container.
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
    });

    builder.Services.RegisterServices(builder.Configuration);

    //builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(typeof(Program).Assembly);

    // responsabilidades transversais
    builder.Services.AddAuthorization(builder.Configuration);
    builder.Services.AddAuthentication(builder.Configuration);

    builder.Services.AddMessageBroker(builder.Configuration);

    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart.WebApi - V1");
            options.RoutePrefix = string.Empty;
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            await app.ApplyMigrations();
        }
    }

    app.UseCors();

    app.UseExceptionHandler(options => { });

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapCarter();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
