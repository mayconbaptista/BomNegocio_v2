
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApiBlock.Extensions
{
    public static class AuthenticationExtension
    {
        public static void AddAuthentication (this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationConf = configuration.GetSection("Authentication:Schemes:Bearer")
                ?? throw new ArgumentNullException("Erro ao Obter a configuração de autenticação");

            var secretKey = authenticationConf.GetValue<string>("IssuerSigningKey")
                ?? throw new ArgumentNullException("Erro ao Obter a chave de autenticação do token jwt");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authenticationConf.GetValue<string>("ValidIssuer"),
                    ValidAudience = authenticationConf.GetValue<string>("ValidAudience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                #if (DEBUG)
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Falha na autenticação: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validado com sucesso.");
                            return Task.CompletedTask;
                        }
                    };
                #endif
            });
        }
    }
}
