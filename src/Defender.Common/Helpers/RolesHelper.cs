using Role = Defender.Common.Models.Roles;

namespace Defender.Common.Helpers;

public static class RolesHelper
{
    public static bool IsAdmin(List<string> roles) =>
        roles.Contains(Role.SuperAdmin) || roles.Contains(Role.Admin);

    public static bool IsSuperAdmin(List<string> roles) =>
        roles.Contains(Role.SuperAdmin);

    public static bool IsUser(List<string> roles) =>
        roles.Contains(Role.User);

    public static bool HasRole(List<string> roles, string role) =>
        roles.Contains(role);

    public static string GetHighestRole(List<string> roles)
    {
        if (roles.Contains(Role.SuperAdmin)) return Role.SuperAdmin;
        if (roles.Contains(Role.Admin)) return Role.Admin;
        if (roles.Contains(Role.User)) return Role.User;

        return Role.Guest;
    }
}
