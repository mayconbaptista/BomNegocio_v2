
using Classifields.Infra.IoC;

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container injection to infra.Ioc
    builder.Services.AddInfrastructure(builder.Configuration);

    //JtwExtencion.AddJtw(builder.Services, builder.Configuration);

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.Logger.LogInformation("Application is running!");

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}catch(Exception ex)
{
    Log.LogError(ex, "An error occurred while starting the application");
}
finally
{
    Console.WriteLine("Application is running!");
}