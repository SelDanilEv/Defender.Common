using Defender.Common.Enums;

namespace Defender.Common.DTOs;

public record AuthCheckDto(Guid UserId, Role HighestRole);
