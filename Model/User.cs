using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Model;

public class User : IdentityUser<Guid>
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public bool IsDeliveryPerson { get; set; }
    
    public ICollection<Delivery> DelivererDeliveries { get; } = new List<Delivery>();
    public ICollection<Delivery> SenderDeliveries { get; } = new List<Delivery>();
    public ICollection<Delivery> RecipientDeliveries { get; } = new List<Delivery>();

    public ICollection<JwtToken> JwtTokens { get; } = new List<JwtToken>();
    
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    
    public User()
    {
    }

    public User(string username, string passwordHash, string firstName, string lastName, string email, string phoneNumber = "", bool isDeliveryPerson = false)
    {
        UserName = username;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        IsDeliveryPerson = isDeliveryPerson;
    }
}