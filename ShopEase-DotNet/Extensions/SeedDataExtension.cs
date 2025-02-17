using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.Models;

public static class SeedDataExtension
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // ✅ Ensure the database is migrated
        await dbContext.Database.MigrateAsync();

        // ✅ Step 1: Seed Roles
        var roles = new List<string> { "Admin", "Vendor", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
                Console.WriteLine($"✅ Role {role} added successfully.");
            }
        }

        // ✅ Step 2: Seed Permissions
        var existingPermissions = await dbContext.Permissions.Select(p => p.Name).ToListAsync();
        var allPermissions = new List<string>
        {
            "ViewCategory", "ManageCategory",
            "ViewProducts", "ManageProducts"
        };

        foreach (var permission in allPermissions)
        {
            if (!existingPermissions.Contains(permission))
            {
                dbContext.Permissions.Add(new Permission
                {
                    Name = permission,
                    Description = $"{permission} permission"
                });
                Console.WriteLine($"✅ Permission {permission} added.");
            }
        }
        await dbContext.SaveChangesAsync();

        // ✅ Step 3: Assign Permissions to Roles
        var rolePermissionsMapping = new Dictionary<string, List<string>>
        {
            { "Admin", new List<string> { "ViewCategory", "ManageCategory", "ViewProducts", "ManageProducts" } },
            { "Vendor", new List<string> { "ViewProducts", "ManageProducts" } },
            { "Customer", new List<string> { "ViewProducts" } }
        };

        foreach (var (roleName, permissions) in rolePermissionsMapping)
        {
            var roleEntity = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (roleEntity == null) continue;

            foreach (var permissionName in permissions)
            {
                var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName);
                if (permission == null) continue;

                bool alreadyAssigned = await dbContext.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleEntity.Id && rp.PermissionId == permission.Id);

                if (!alreadyAssigned)
                {
                    dbContext.RolePermissions.Add(new RolePermission
                    {
                        RoleId = roleEntity.Id,
                        PermissionId = permission.Id
                    });
                    Console.WriteLine($"✅ Permission {permissionName} assigned to {roleName}.");
                }
            }
        }
        await dbContext.SaveChangesAsync();

        // ✅ Step 4: Seed Default Admin User
        string adminEmail = "admin@shopease.com";
        string adminPassword = "Admin@123";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "ShopEase",
                Profession="Admin",
                LastName = "Admin",
                IsVerified = true
            };

            var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"✅ Admin user created and assigned role.");
            }
            else
            {
                Console.WriteLine($"❌ Failed to create admin user.");
            }
        }
    }
}
