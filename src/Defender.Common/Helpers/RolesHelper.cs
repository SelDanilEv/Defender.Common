using Defender.Common.Consts;
using Defender.Common.Enums;

namespace Defender.Common.Helpers;

public static class RolesHelper
{
    private static readonly List<string> SuperAdminRoles =
        new List<string> { Roles.SuperAdmin, Roles.Admin, Roles.User, Roles.Guest };
    private static readonly List<string> AdminRoles =
        new List<string> { Roles.Admin, Roles.User, Roles.Guest };
    private static readonly List<string> UserRoles =
        new List<string> { Roles.User, Roles.Guest };
    private static readonly List<string> GuestRoles =
        new List<string> { Roles.Guest };

    public static bool IsAdmin(List<string> roles) =>
        roles.Contains(Roles.SuperAdmin) || roles.Contains(Roles.Admin);

    public static bool IsSuperAdmin(List<string> roles) =>
        roles.Contains(Roles.SuperAdmin);

    public static bool IsUser(List<string> roles) =>
        roles.Contains(Roles.User);

    public static bool HasRole(List<string> roles, string role) =>
        roles.Contains(role);

    public static bool HasRole(List<string> roles, Role role) =>
        HasRole(roles, role.ToString());

    public static Role GetHighestRole(List<string> roles)
    {
        if (roles.Contains(Roles.SuperAdmin)) return Role.SuperAdmin;
        if (roles.Contains(Roles.Admin)) return Role.Admin;
        if (roles.Contains(Roles.User)) return Role.User;

        return Role.Guest;
    }

    public static string GetHighestRoleString(List<string> roles)
    {
        if (roles.Contains(Roles.SuperAdmin)) return Roles.SuperAdmin;
        if (roles.Contains(Roles.Admin)) return Roles.Admin;
        if (roles.Contains(Roles.User)) return Roles.User;

        return Roles.Guest;
    }

    public static List<string> GetRolesList(Role role)
    {
        return role switch
        {
            Role.SuperAdmin => SuperAdminRoles,
            Role.Admin => AdminRoles,
            Role.User => UserRoles,
            _ => GuestRoles
        };
    }
}
