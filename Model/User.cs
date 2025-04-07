using System.ComponentModel.DataAnnotations;

namespace Model;

public class User : BaseEntity
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public bool IsDeliveryPerson { get; set; }
    
    public ICollection<Delivery> DelivererDeliveries { get; set; } = new List<Delivery>();
    public ICollection<Delivery> SenderDeliveries { get; set; } = new List<Delivery>();
    
    public User()
    {
    }

    public User(string username, string password, string firstName, string lastName, string email, string phoneNumber = "", bool isDeliveryPerson = false)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        IsDeliveryPerson = isDeliveryPerson;
    }
}