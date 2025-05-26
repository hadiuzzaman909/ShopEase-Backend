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
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // If you use credentials like cookies or auth headers
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
            // Apply the specific origins policy here instead of AllowAll
            app.UseCors("AllowSpecificOrigins");
            return app;
        }
    }
}
