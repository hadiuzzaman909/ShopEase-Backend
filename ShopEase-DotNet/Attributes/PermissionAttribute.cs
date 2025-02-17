using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Models;
using ShopEase.Services.IServices;

namespace ShopEase.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        private readonly string _permission;
        public PermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

            var user = await userManager.GetUserAsync(context.HttpContext.User);
            if (user == null || !await permissionService.HasPermissionAsync(user.Id, _permission))
            {
                context.Result = new ForbidResult();
            }
        }

    }

}