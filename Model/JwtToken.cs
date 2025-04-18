namespace Model;

public class JwtToken : BaseEntity
{
    public JwtToken()
    {
    }

    public JwtToken(string token)
    {
        Token = token;
    }

    public string Token { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}