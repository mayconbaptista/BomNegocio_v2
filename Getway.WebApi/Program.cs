using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// CORS so existe quando front e API estao em origens diferentes (ng serve em
// localhost:4200 -> gateway em localhost:8080). Na AWS o CloudFront serve
// Angular e /api na MESMA origem, entao a secao fica vazia e nenhuma origem
// externa e liberada. Nunca AllowAnyOrigin: qualquer site poderia chamar a API
// a partir do browser do usuario.
var cors = builder.Configuration
    .GetSection("CorsPolicies:AllowSpecificOrigins")
    .Get<CorsPolicySettings>() ?? CorsPolicySettings.None;

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicySettings.PolicyName, policy =>
    {
        policy.WithOrigins(cors.Origins)
              .WithMethods(cors.Methods)
              .WithHeaders(cors.Headers);

        if (cors.AllowCredentials)
        {
            policy.AllowCredentials();
        }
    });
});

builder
    .Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors(CorsPolicySettings.PolicyName);

// Health check usado pelo deploy.sh. No WebApplication os endpoints executam
// no FIM do pipeline, e o UseOcelot e terminal: sem ShortCircuit() o Ocelot
// responderia 404 aqui. ShortCircuit (.NET 8) executa o endpoint logo apos o
// roteamento, sem passar pelo resto do pipeline (CORS, Ocelot).
app.MapGet("/healthz", () => Results.Ok("ok")).ShortCircuit();

await app.UseOcelot();

await app.RunAsync();

internal sealed record CorsPolicySettings
{
    public const string PolicyName = "CorsPolicy";

    public static readonly CorsPolicySettings None = new();

    public string[] Origins { get; init; } = [];
    public string[] Methods { get; init; } = ["GET"];
    public string[] Headers { get; init; } = ["Content-Type"];
    public bool AllowCredentials { get; init; }
}
