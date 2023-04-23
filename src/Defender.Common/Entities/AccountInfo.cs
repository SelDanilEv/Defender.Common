using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.Entities;

public class AccountInfo : IBaseModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsEmailVerified { get; set; }
    public List<string> Roles { get; set; } = new List<string>();

    [BsonIgnore]
    public bool IsAdmin => Roles.Contains(Models.Roles.SuperAdmin) || Roles.Contains(Models.Roles.Admin);

    [BsonIgnore]
    public bool IsSuperAdmin => Roles.Contains(Models.Roles.SuperAdmin);

    public bool HasRole(string role) => Roles.Contains(role);

    public string GetHighestRole()
    {
        if (Roles.Contains(Models.Roles.SuperAdmin)) return Models.Roles.SuperAdmin;
        if (Roles.Contains(Models.Roles.Admin)) return Models.Roles.Admin;
        if (Roles.Contains(Models.Roles.User)) return Models.Roles.User;

        return string.Empty;
    }

}
