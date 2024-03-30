using Defender.Common.DTOs;

namespace Defender.Common.Interfaces;

public interface ICurrentAccountAccessor
{
    Guid GetAccountId();
    List<string> GetRoles();
    string GetHighestRole();
    string? Token { get; }
}
