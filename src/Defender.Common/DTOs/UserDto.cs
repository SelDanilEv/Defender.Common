namespace Defender.Common.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Nickname { get; set; }
    public DateTime? CreatedDate { get; set; }
}
