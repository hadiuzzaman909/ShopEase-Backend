namespace Vellora.ECommerce.API.Extensions
{
    public static class CorsServiceExtension
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins(allowedOrigins) // Use specific allowed origins from config
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });

                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }

        public static WebApplication UseCorsPolicies(this WebApplication app)
        {
            app.UseCors("AllowAll"); // You can change this to "AllowSpecificOrigins"
            return app;
        }
    }

}