using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }

    public bool IsPhoneVerified { get; set; }

    public bool IsEmailVerified { get; set; }

    public List<string> Roles { get; set; } = new List<string>();

    [BsonIgnore]
    public bool IsAdmin => this.Roles.Contains(Models.Roles.SuperAdmin) || this.Roles.Contains(Models.Roles.Admin);

    [BsonIgnore]
    public bool IsSuperAdmin => this.Roles.Contains(Models.Roles.SuperAdmin);
}
