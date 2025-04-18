using System.ComponentModel.DataAnnotations;

namespace Model;

public class RefreshToken
{
    public RefreshToken()
    {
    }

    public RefreshToken(string token, DateTime expires)
    {
        Token = token;
        Expires = expires;
    }

    [Key] public string Token { get; set; } = "";

    public DateTime Expires { get; set; }
}