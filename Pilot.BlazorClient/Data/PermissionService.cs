using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.Data;

public static class PermissionService
{
    public static bool HasPermission(this Permission permission, Permission permissionToCheck)
    {
        return permission.HasFlag(permissionToCheck) || permission.HasFlag(Permission.All);
    }
}