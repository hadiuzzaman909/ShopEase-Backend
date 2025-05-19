using Microsoft.AspNetCore.Identity;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Models;

namespace Vellora.ECommerce.API.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Register Identity with `ApplicationRole`
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity Options
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 1;
            });

            return services;
        }
    }
}