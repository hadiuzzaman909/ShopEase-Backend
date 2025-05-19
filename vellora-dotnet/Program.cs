using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Extensions;
using Vellora.ECommerce.API.Services.IServices;
using Vellora.ECommerce.API.Services;
using Vellora.ECommerce.API.Utils;
using Vellora.ECommerce.API.Repositories.IRepositories;
using Vellora.ECommerce.API.Repositories;
using Vellora.ECommerce.API.Profiles;
using Vellora.ECommerce.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(connectionString));    

// Register Identity Using Extension
builder.Services.ConfigureIdentity(builder.Configuration);

// Register Authentication, CORS, and Utilities
builder.Services.ConfigureJwtAuth(builder.Configuration);
builder.Services.AddCorsPolicies(builder.Configuration);
builder.Services.AddSingleton<JwtUtils>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register Repository Layer
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>(); // Register Order Service

// Add Authorization Policies (Role-Based Access Control)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("VendorOnly", policy => policy.RequireRole("Vendor"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// Add Controllers
builder.Services.AddControllers();

// Use Swagger Extension
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Run Database Seeding AFTER the app is built
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedDataExtension.SeedRolesAndUsersAsync(serviceProvider);
}

//Middleware 
app.UseMiddleware<ErrorHandlerMiddleware>();

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseCorsPolicies();
app.UseAuthentication();
app.UseAuthorization();

// Conditionally Use Swagger Middleware based on Environment
app.UseSwaggerMiddleware(app.Environment);  

app.MapControllers();
app.Run();