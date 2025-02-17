namespace ShopEase.Extensions
{
    public static class CorsServiceExtension
    {
        // Configure CORS
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            return services;
        }

        // Apply CORS
        public static WebApplication UseCorsPolicies(this WebApplication app)
        {
            app.UseCors("AllowAll");
            return app;
        }
    }
}