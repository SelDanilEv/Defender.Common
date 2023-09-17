namespace Defender.Common.DTOs;

using Role = Models.Roles;

public class AccountDto
{
    public Guid Id { get; set; }

    public bool IsPhoneVerified { get; set; }

    public bool IsEmailVerified { get; set; }

    public List<string> Roles { get; set; } = new List<string>();

    public bool IsAdmin => Roles.Contains(Role.SuperAdmin) || Roles.Contains(Role.Admin);

    public bool IsSuperAdmin => Roles.Contains(Role.SuperAdmin);

    public bool HasRole(string role) => Roles.Contains(role);

    public string GetHighestRole()
    {
        if (Roles.Contains(Role.SuperAdmin)) return Role.SuperAdmin;
        if (Roles.Contains(Role.Admin)) return Role.Admin;
        if (Roles.Contains(Role.User)) return Role.User;

        return Role.Guest;
    }
}
