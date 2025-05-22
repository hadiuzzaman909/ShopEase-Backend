using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Vellora.ECommerce.API.Services.IServices;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserPermissionService _userPermissionService;

    public PermissionHandler(IUserPermissionService userPermissionService)
    {
        _userPermissionService = userPermissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            context.Fail();
            return;
        }

        // Check if user has the required permission
        bool hasPermission = await _userPermissionService.UserHasPermissionAsync(userId, requirement.PermissionName);

        if (hasPermission)
            context.Succeed(requirement);
        else
            context.Fail();
    }
}
