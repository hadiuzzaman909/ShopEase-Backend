using Microsoft.AspNetCore.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; private set; }

    public PermissionRequirement(string permissionName)
    {
        PermissionName = permissionName;
    }
}