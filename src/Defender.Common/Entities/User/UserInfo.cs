namespace Defender.Common.Entities.User;

public class UserInfo : JwtInfo
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Nickname { get; set; }
    public string? PasswordHash { get; set; }

    public bool IsPhoneVerified { get; set; }
    public bool IsEmailVerified { get; set; }

    public DateTime? CreatedDate { get; set; }
}
