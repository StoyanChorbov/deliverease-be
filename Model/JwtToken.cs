namespace Model;

public class JwtToken : BaseEntity
{
    public string Token { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    public JwtToken()
    {
    }
    
    public JwtToken(string token)
    {
        Token = token;
    }
}