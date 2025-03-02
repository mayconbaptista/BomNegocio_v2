namespace Auth.WebApi
{
    public static class InjectionContainer
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            #region "Services"
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            #endregion

            return services;
        }
    }
}
