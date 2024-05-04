using Defender.Common.Enums;

namespace Defender.Common.Interfaces;

public interface ICurrentAccountAccessor
{
    Guid GetAccountId();
    List<string> GetRoles();
    Role GetHighestRole();
    bool HasRole(Role role);
    string? Token { get; }
}
