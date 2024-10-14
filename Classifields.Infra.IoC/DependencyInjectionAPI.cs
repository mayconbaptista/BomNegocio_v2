using BomNegocio.Application.Mappings;
using Classifields.Application.Interfaces;
using Classifields.Application.Services;
using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;
using Classifields.Infra.Data.Identity;
using Classifields.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Classifields.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BNContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")),
                b => b.MigrationsAssembly(typeof(BNContext).Assembly.FullName)));

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(consfig =>
            {
                consfig.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            });

            #region "DI-Repository"
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAdvertiserRepository, AdvertiserRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IEvaluetionRepository, EvaluetionRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWisheRepository, WisheRepository>();
            #endregion

            #region "DI-Services"
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IAdvertiserService, AdvertiserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            #endregion

            #region "DI-autorization"
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BNContext>()
                .AddDefaultTokenProviders();
            #endregion

            return services;
        }
    }
}
