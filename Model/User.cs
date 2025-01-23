using System.ComponentModel.DataAnnotations;

namespace Model;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
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
    
    public string? Phone { get; set; }

    public User()
    {
    }

    public User(string username, string password, string firstName, string lastName, string email)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public User(string username, string password, string firstName, string lastName, string email, string? phone)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }
}