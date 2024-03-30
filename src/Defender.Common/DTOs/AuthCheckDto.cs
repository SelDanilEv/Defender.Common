namespace Defender.Common.DTOs;

public record AuthCheckDto(Guid UserId, string HighestRole);
