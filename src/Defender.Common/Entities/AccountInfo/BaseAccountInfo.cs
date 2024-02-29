using Defender.Common.Helpers;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.Entities.AccountInfo;

[BsonIgnoreExtraElements]
public record BaseAccountInfo : IBaseModel
{
    [BsonId]
    public Guid Id { get; set; }

    public List<string> Roles { get; set; } = new List<string>();

    [BsonIgnore]
    public bool IsAdmin => RolesHelper.IsAdmin(Roles);

    [BsonIgnore]
    public bool IsSuperAdmin => RolesHelper.IsSuperAdmin(Roles);

    public bool HasRole(string role) => RolesHelper.HasRole(Roles, role);

    public string GetHighestRole() => RolesHelper.GetHighestRole(Roles);
}
