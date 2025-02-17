using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.Extensions;
using ShopEase.Services.IServices;
using ShopEase.Services;
using ShopEase.Utils;
using ShopEase.Repositories.IRepositories;
using ShopEase.Repositories;
using ShopEase.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Identity Using Extension
builder.Services.ConfigureIdentity(builder.Configuration);

// Register Authentication, CORS, and Utilities
builder.Services.ConfigureJwtAuth(builder.Configuration);
builder.Services.AddCorsPolicies();
builder.Services.AddSingleton<JwtUtils>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ✅ Register Repository Layer 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>(); 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run Database Seeding AFTER the app is built
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedDataExtension.SeedRolesAndUsersAsync(serviceProvider);
}

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseCorsPolicies();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopEase API v1");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.Run();